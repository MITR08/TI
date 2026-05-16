using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace RsaSignatureLab;

public sealed record SplitSignedTextResult(string Message, BigInteger Signature);

public sealed record RsaValidatedParams(BigInteger P, BigInteger Q, BigInteger D, BigInteger R, BigInteger Phi, BigInteger E);

/// <summary>
/// Логика ЭЦП RSA и хеша по условию (H₀ = 100, модуль n = r = p·q), перенесённая без изменений с веб-версии.
/// </summary>
public static class RsaCrypto
{
    private static readonly BigInteger H0 = 100;

    private const string CyrillicAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public static BigInteger ParseDecimalStrict(string? str)
    {
        var s = (str ?? string.Empty).Trim();
        if (!Regex.IsMatch(s, @"^\d+$"))
            throw new InvalidOperationException("Введите целое число в 10-й системе.");

        return BigInteger.Parse(s, CultureInfo.InvariantCulture);
    }

    private static BigInteger Gcd(BigInteger a, BigInteger b)
    {
        var x = a < BigInteger.Zero ? -a : a;
        var y = b < BigInteger.Zero ? -b : b;

        while (y != BigInteger.Zero)
        {
            var t = x % y;
            x = y;
            y = t;
        }

        return x;
    }

    public static BigInteger ModPow(BigInteger baseValue, BigInteger exp, BigInteger mod)
    {
        if (mod == BigInteger.One)
            return BigInteger.Zero;

        var b = ((baseValue % mod) + mod) % mod;
        var e = exp;
        var result = BigInteger.One;

        while (e > BigInteger.Zero)
        {
            if ((e & BigInteger.One) == BigInteger.One)
                result = (result * b) % mod;
            b = (b * b) % mod;
            e >>= 1;
        }

        return result;
    }

    public static BigInteger ModInv(BigInteger a, BigInteger mod)
    {
        BigInteger t = BigInteger.Zero;
        BigInteger newT = BigInteger.One;
        BigInteger r = mod;
        var newR = ((a % mod) + mod) % mod;

        while (newR != BigInteger.Zero)
        {
            var q = r / newR;

            var oldT = t;
            var oldNewT = newT;
            t = newT;
            newT = oldT - q * oldNewT;

            var oldR = r;
            var oldNewR = newR;
            r = newR;
            newR = oldR - q * oldNewR;
        }

        if (r != BigInteger.One)
            throw new InvalidOperationException("Обратного элемента не существует (gcd ≠ 1).");

        if (t < BigInteger.Zero)
            t += mod;

        return t;
    }

    private static readonly BigInteger[] MillerRabinSmallPrimes =
        { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37 };

    private static readonly BigInteger[] MillerRabinBases =
        { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };

    public static bool IsProbablePrime(BigInteger n)
    {
        if (n < 2)
            return false;

        foreach (var p in MillerRabinSmallPrimes)
        {
            if (n == p)
                return true;
        }

        foreach (var p in MillerRabinSmallPrimes)
        {
            if (n % p == BigInteger.Zero)
                return false;
        }

        var d = n - BigInteger.One;
        var s = BigInteger.Zero;
        while ((d & BigInteger.One) == BigInteger.Zero)
        {
            d >>= 1;
            s += BigInteger.One;
        }

        foreach (var a0 in MillerRabinBases)
        {
            var a = a0 % n;
            if (a == BigInteger.Zero)
                continue;

            var x = ModPow(a, d, n);
            if (x == BigInteger.One || x == n - BigInteger.One)
                continue;

            var cont = false;
            for (var r = BigInteger.One; r < s; r += BigInteger.One)
            {
                x = (x * x) % n;
                if (x == n - BigInteger.One)
                {
                    cont = true;
                    break;
                }
            }

            if (cont)
                continue;

            return false;
        }

        return true;
    }

    private static BigInteger CharToMi(Rune rune)
    {
        var ruTextInfo = CultureInfo.GetCultureInfo("ru-RU").TextInfo;
        var upRu = ruTextInfo.ToUpper(rune.ToString());

        for (var i = 0; i < CyrillicAlphabet.Length; i++)
        {
            if (upRu == CyrillicAlphabet[i].ToString())
                return new BigInteger(i + 1);
        }

        var cp = (uint)rune.Value;
        if ((cp >= 0x41 && cp <= 0x5a) || (cp >= 0x61 && cp <= 0x7a))
        {
            var upLat = cp >= 0x61 ? cp - 0x20 : cp;
            return new BigInteger(upLat);
        }

        return cp;
    }

    public static BigInteger HashMessage32(string text, BigInteger n)
    {
        if (n <= BigInteger.One)
            throw new InvalidOperationException("Модуль n = p·q должен быть > 1.");

        if (text.Length == 0)
            return H0;

        var h = H0;
        foreach (var rune in text.EnumerateRunes())
        {
            var mi = CharToMi(rune);
            var t = (h + mi) % n;
            h = (t * t) % n;
        }

        return h;
    }

    public static RsaValidatedParams ValidateParams(string pStr, string qStr, string dStr)
    {
        var p = ParseDecimalStrict(pStr);
        var q = ParseDecimalStrict(qStr);
        var d = ParseDecimalStrict(dStr);

        if (p < 3)
            throw new InvalidOperationException("p должно быть простым и не меньше 3.");
        if (q < 3)
            throw new InvalidOperationException("q должно быть простым и не меньше 3.");
        if (!IsProbablePrime(p))
            throw new InvalidOperationException("p не является простым.");
        if (!IsProbablePrime(q))
            throw new InvalidOperationException("q не является простым.");
        if (p == q)
            throw new InvalidOperationException("p и q должны быть различными простыми.");

        var r = p * q;
        var phi = (p - BigInteger.One) * (q - BigInteger.One);

        if (d <= BigInteger.One || d >= phi)
            throw new InvalidOperationException($"d должно быть в диапазоне 2..φ(r)-1, где φ(r) = {phi.ToString(CultureInfo.InvariantCulture)}.");
        if (Gcd(d, phi) != BigInteger.One)
            throw new InvalidOperationException($"d должно быть взаимно простым с φ(r) = {phi.ToString(CultureInfo.InvariantCulture)}.");

        BigInteger e;
        try
        {
            e = ModInv(d, phi);
        }
        catch
        {
            throw new InvalidOperationException("Не удалось вычислить открытую экспоненту e = d⁻¹ mod φ(r).");
        }

        if ((e * d) % phi != BigInteger.One)
            throw new InvalidOperationException("Внутренняя ошибка: e·d ≢ 1 (mod φ(r)).");

        return new RsaValidatedParams(p, q, d, r, phi, e);
    }

    public static SplitSignedTextResult SplitSignedText(string full)
    {
        var normalized = full.Replace("\r\n", "\n", StringComparison.Ordinal);
        var lines = normalized.Split('\n');
        if (lines.Length < 2)
            throw new InvalidOperationException("Файл должен заканчиваться строкой с подписью (целое число в 10-й с.с.).");

        var last = lines[^1].Trim();
        if (!Regex.IsMatch(last, @"^\d+$"))
            throw new InvalidOperationException("Последняя строка файла должна содержать только цифры подписи.");

        var message = string.Join('\n', lines[..^1]);
        return new SplitSignedTextResult(message, BigInteger.Parse(last, CultureInfo.InvariantCulture));
    }

    /// <returns>Вычисленная подпись S и промежуточные значения.</returns>
    public static (BigInteger S, BigInteger M, BigInteger MSign, BigInteger RecoveredCheck) ComputeSignature(string text, RsaValidatedParams p)
    {
        var r = p.R;
        var d = p.D;
        var e = p.E;
        var m = HashMessage32(text, r);
        var mSign = ((m % r) + r) % r;

        if (Gcd(mSign, r) != BigInteger.One)
        {
            throw new InvalidOperationException(
                "gcd(h(M), r) ≠ 1 — для данного хеша стандартная RSA-подпись неприменима. Измените текст или p, q.");
        }

        var s = ModPow(mSign, d, r);
        var recovered = ModPow(s, e, r);
        return (s, m, mSign, recovered);
    }

    /// <returns>Объяснение и числовые столбцы для интерфейса.</returns>
    public static VerifiedBundle VerifySignedFile(string signedFileContent, RsaValidatedParams p)
    {
        var full = signedFileContent;
        var parsed = SplitSignedText(full);
        var message = parsed.Message;
        var sigS = parsed.Signature;

        var r = p.R;
        var e = p.E;

        var mPrime = HashMessage32(message, r);
        var mPrimeSign = ((mPrime % r) + r) % r;
        var mFromSig = ModPow(sigS, e, r);
        var ok = mPrimeSign == mFromSig;

        var rStr = r.ToString(CultureInfo.InvariantCulture);
        var mPrimeStr = mPrime.ToString(CultureInfo.InvariantCulture);
        var mSignStr = mPrimeSign.ToString(CultureInfo.InvariantCulture);
        var mFromSigStr = mFromSig.ToString(CultureInfo.InvariantCulture);

        string reason;
        if (ok)
        {
            reason =
                "Сравниваются величины из условия проверки ЭЦП RSA: h(M′) mod r и S^e mod r (r = p·q = " + rStr + "). "
                + "Они совпадают и равны " + mSignStr + "..";

            if (mPrime != mPrimeSign)
            {
                reason +=
                    " По формуле 3.2 h(M′) = " + mPrimeStr + " (для пустого сообщения это может быть H₀ = 100); "
                    + "для сравнения с подписью используется остаток " + mPrimeStr + " mod " + rStr + " = " + mSignStr + ".";
            }
        }
        else
        {
            reason =
                "Сравниваются h(M′) mod r и S^e mod r при r = " + rStr + ". Сейчас h(M′) mod r = " + mSignStr + ", а S^e mod r = "
                + mFromSigStr + " — "
                + "они различны "
                ;

            if (mPrime != mPrimeSign)
                reason += " (h(M′) по формуле 3.2: " + mPrimeStr + ".)";
        }

        return new VerifiedBundle(
            ok,
            reason,
            mPrimeStr,
            mSignStr,
            mFromSigStr,
            rStr,
            sigS.ToString(CultureInfo.InvariantCulture),
            e.ToString(CultureInfo.InvariantCulture));
    }
}

public sealed record VerifiedBundle(
    bool Ok,
    string Reason,
    string MPrime,
    string MPrimeSign,
    string MFromSig,
    string R,
    string S,
    string E);
