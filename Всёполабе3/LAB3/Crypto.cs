using System.Numerics;

namespace Lab3WinForms;

internal static class Crypto
{
    private const int U16Max = 65535;
    private static readonly BigInteger Mask64 = (BigInteger.One << 64) - 1;

    internal static BigInteger ParseDecimalStrict(string? str)
    {
        var s = (str ?? "").Trim();
        if (s.Length == 0 || !s.All(char.IsDigit))
            throw new InvalidOperationException("Введите целое число в 10-й системе.");
        return BigInteger.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
    }

    internal static BigInteger Gcd(BigInteger a, BigInteger b)
    {
        var x = a < 0 ? -a : a;
        var y = b < 0 ? -b : b;
        while (y != 0)
        {
            var t = x % y;
            x = y;
            y = t;
        }
        return x;
    }

    internal sealed class KStream
    {
        private readonly BigInteger _p;
        private readonly BigInteger _kFirst;
        private readonly BigInteger _phi;
        private readonly BigInteger _hi;
        private BigInteger _state;
        private bool _first = true;

        internal KStream(BigInteger p, BigInteger kFirst)
        {
            _p = p;
            _kFirst = kFirst;
            _phi = p - 1;
            _hi = p - 2;
            if (_hi < 1)
                throw new InvalidOperationException("p слишком мало.");
            _state = (kFirst * 0x9e3779b97f4a7c15 + p * 0x85ebca6b2bd3e8d9) & Mask64;
            if (_state == 0)
                _state = 0xdeadbeef00000001;
        }

        internal BigInteger Next()
        {
            if (_first)
            {
                _first = false;
                return _kFirst;
            }
            return NextFromPrng();
        }

        private BigInteger NextFromPrng()
        {
            var span = _hi;
            for (var t = 0; t < 100000; t++)
            {
                _state = (_state * 6364136223846793005 + 1442695040888963407) & Mask64;
                var cand = (_state % span) + 1;
                if (Gcd(cand, _phi) == 1)
                    return cand;
            }
            throw new InvalidOperationException("ГПСЧ не смог выдать k с gcd(k, p−1)=1.");
        }
    }

    internal static BigInteger ModPow(BigInteger baseVal, BigInteger exp, BigInteger mod)
    {
        if (mod == 1)
            return 0;
        return BigInteger.ModPow(((baseVal % mod) + mod) % mod, exp, mod);
    }

    internal static BigInteger ModInv(BigInteger a, BigInteger mod)
    {
        BigInteger t = 0, newT = 1;
        BigInteger r = mod, newR = ((a % mod) + mod) % mod;
        while (newR != 0)
        {
            var q = r / newR;
            (t, newT) = (newT, t - q * newT);
            (r, newR) = (newR, r - q * newR);
        }
        if (r != 1)
            throw new InvalidOperationException("Обратного элемента не существует (gcd != 1).");
        if (t < 0)
            t += mod;
        return t;
    }

    internal static bool IsProbablePrime(BigInteger n)
    {
        if (n < 2)
            return false;
        BigInteger[] small = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37];
        if (small.Contains(n))
            return true;
        foreach (var p in small)
        {
            if (n % p == 0)
                return false;
        }

        var d = n - 1;
        BigInteger s = 0;
        while ((d & 1) == 0)
        {
            d >>= 1;
            s += 1;
        }

        BigInteger[] bases = [2, 325, 9375, 28178, 450775, 9780504, 1795265022];
        foreach (var a0 in bases)
        {
            var a = a0 % n;
            if (a == 0)
                continue;
            var x = ModPow(a, d, n);
            if (x == 1 || x == n - 1)
                continue;
            var cont = false;
            for (BigInteger r = 1; r < s; r += 1)
            {
                x = (x * x) % n;
                if (x == n - 1)
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

    internal static Dictionary<BigInteger, BigInteger> Factorize(BigInteger n)
    {
        var factors = new Dictionary<BigInteger, BigInteger>();
        var x = n;
        BigInteger d = 2;
        while (d * d <= x)
        {
            while (x % d == 0)
            {
                factors[d] = factors.GetValueOrDefault(d, 0) + 1;
                x /= d;
            }
            d = d == 2 ? 3 : d + 2;
        }
        if (x > 1)
            factors[x] = factors.GetValueOrDefault(x, 0) + 1;
        return factors;
    }

    internal static (BigInteger Phi, List<BigInteger> PrimeFactors) PrimitiveRootCandidates(BigInteger p)
    {
        var phi = p - 1;
        var fac = Factorize(phi);
        var primeFactors = fac.Keys.ToList();
        return (phi, primeFactors);
    }

    internal static bool IsPrimitiveRoot(BigInteger g, BigInteger p, BigInteger phi, List<BigInteger> primeFactors)
    {
        if (g <= 1 || g >= p)
            return false;
        foreach (var q in primeFactors)
        {
            if (ModPow(g, phi / q, p) == 1)
                return false;
        }
        return true;
    }

    internal static List<BigInteger> FindAllPrimitiveRoots(BigInteger p)
    {
        if (!IsProbablePrime(p))
            throw new InvalidOperationException("p должно быть простым числом.");
        var (phi, primeFactors) = PrimitiveRootCandidates(p);

        BigInteger? g0 = null;
        for (var g = (BigInteger)2; g < p; g += 1)
        {
            if (IsPrimitiveRoot(g, p, phi, primeFactors))
            {
                g0 = g;
                break;
            }
        }
        if (g0 is null)
            return [];

        var roots = new List<BigInteger>();
        for (BigInteger t = 1; t <= phi; t += 1)
        {
            if (Gcd(t, phi) == 1)
                roots.Add(ModPow(g0.Value, t, p));
        }
        var uniq = roots.Select(r => r.ToString()).Distinct().Select(BigInteger.Parse).OrderBy(x => x).ToList();
        return uniq;
    }

    private static ushort BigIntegerToU16Checked(BigInteger x, string label)
    {
        if (x < 0 || x > U16Max)
            throw new InvalidOperationException($"{label} не помещается в 2 байта (нужно 0..65535); уменьши p.");
        return (ushort)x;
    }

    internal static byte[] MakeEncryptedBlob(IReadOnlyList<(BigInteger A, BigInteger B)> pairs)
    {
        var count = pairs.Count;
        var outBytes = new byte[count * 4];
        var off = 0;
        for (var i = 0; i < count; i++)
        {
            var (a, b) = pairs[i];
            var ua = BigIntegerToU16Checked(a, "a");
            var ub = BigIntegerToU16Checked(b, "b");
            outBytes[off++] = (byte)(ua & 0xff);
            outBytes[off++] = (byte)((ua >> 8) & 0xff);
            outBytes[off++] = (byte)(ub & 0xff);
            outBytes[off++] = (byte)((ub >> 8) & 0xff);
        }
        return outBytes;
    }

    internal static List<(BigInteger A, BigInteger B)> ParseEncrypted(byte[] bytes)
    {
        if (bytes.Length == 0)
            return [];
        if (bytes.Length % 4 != 0)
            throw new InvalidOperationException("Неверная длина файла: ожидается 4·N байт (пары a_i, b_i по 2 байта).");
        var n = bytes.Length / 4;
        var pairs = new List<(BigInteger, BigInteger)>(n);
        for (var i = 0; i < n; i++)
        {
            var baseIdx = i * 4;
            var a = (uint)(bytes[baseIdx] | (bytes[baseIdx + 1] << 8));
            var b = (uint)(bytes[baseIdx + 2] | (bytes[baseIdx + 3] << 8));
            pairs.Add((a, b));
        }
        return pairs;
    }

    internal static string CiphertextPairsDecimalPreview(byte[] bytes, int limitPairs = 512)
    {
        if (bytes.Length == 0)
            return "";
        if (bytes.Length % 4 != 0)
            return "";
        var n = bytes.Length / 4;
        var show = Math.Min(n, limitPairs);
        var parts = new List<string>(show);
        for (var i = 0; i < show; i++)
        {
            var baseIdx = i * 4;
            var a = bytes[baseIdx] | (bytes[baseIdx + 1] << 8);
            var b = bytes[baseIdx + 2] | (bytes[baseIdx + 3] << 8);
            parts.Add($"{a} {b}");
        }
        var suffix = n > limitPairs ? $" ... (показано {limitPairs} пар из {n})" : "";
        return string.Join("  ", parts) + suffix;
    }

    internal static (BigInteger P, BigInteger X, BigInteger K, BigInteger G) ValidateEncryptParams(string pStr, string xStr, string kStr, string gStr)
    {
        var p = ParseDecimalStrict(pStr);
        var x = ParseDecimalStrict(xStr);
        var k = ParseDecimalStrict(kStr);
        var g = ParseDecimalStrict(gStr);

        if (!IsProbablePrime(p))
            throw new InvalidOperationException("p должно быть простым числом.");
        if (p <= 257)
            throw new InvalidOperationException("p должно быть > 257 (чтобы шифровать байты 0..255).");
        if (p > 65536)
            throw new InvalidOperationException("p должно быть ≤ 65536: a и b кодируются по 2 байта (0..65535).");
        if (x < 2 || x > p - 2)
            throw new InvalidOperationException("x по методичке: целое, 1 < x < p−1, т.е. 2 ≤ x ≤ p−2.");
        if (k <= 0 || k >= p - 1)
            throw new InvalidOperationException("k должно быть в диапазоне 1..p-2.");
        if (Gcd(k, p - 1) != 1)
            throw new InvalidOperationException("k должно быть взаимно простым с (p−1).");
        if (g <= 1 || g >= p)
            throw new InvalidOperationException("g должно быть в диапазоне 2..p-1.");

        var (phi, primeFactors) = PrimitiveRootCandidates(p);
        if (!IsPrimitiveRoot(g, p, phi, primeFactors))
            throw new InvalidOperationException("Выбранный g не является первообразным корнем по модулю p.");

        return (p, x, k, g);
    }

    internal static byte[] EncryptBytes(byte[] plain, BigInteger p, BigInteger x, BigInteger k, BigInteger g)
    {
        var y = ModPow(g, x, p);
        var nextK = new KStream(p, k);
        var pairs = new List<(BigInteger A, BigInteger B)>(plain.Length);
        foreach (var by in plain)
        {
            var ki = nextK.Next();
            var ai = ModPow(g, ki, p);
            var yk = ModPow(y, ki, p);
            var m = (BigInteger)by;
            var bi = (m * yk) % p;
            pairs.Add((ai, bi));
        }
        return MakeEncryptedBlob(pairs);
    }

    internal static byte[] DecryptBytes(byte[] enc, BigInteger p, BigInteger x)
    {
        if (enc.Length == 0)
            return [];
        var pairs = ParseEncrypted(enc);
        var outBytes = new byte[pairs.Count];
        for (var i = 0; i < pairs.Count; i++)
        {
            var (a, b) = pairs[i];
            if (a <= 0 || a >= p)
                throw new InvalidOperationException($"a[{i}] вне диапазона [1, p-1].");
            if (b < 0 || b >= p)
                throw new InvalidOperationException($"b[{i}] вне диапазона [0, p-1].");
            var ax = ModPow(a, x, p);
            var invAx = ModInv(ax, p);
            var m = (b * invAx) % p;
            if (m < 0 || m > 255)
                throw new InvalidOperationException("Расшифровка дала байт вне диапазона 0..255 (проверь x/p).");
            outBytes[i] = (byte)m;
        }
        return outBytes;
    }

    internal static void ValidateDecryptParams(string pStr, string xStr)
    {
        var p = ParseDecimalStrict(pStr);
        var x = ParseDecimalStrict(xStr);
        if (!IsProbablePrime(p))
            throw new InvalidOperationException("p должно быть простым числом.");
        if (p <= 257 || p > 65536)
            throw new InvalidOperationException("p должно быть в диапазоне простых: > 257 и ≤ 65536 (формат 2 байта на число).");
        if (x < 2 || x > p - 2)
            throw new InvalidOperationException("x: по методичке 2 ≤ x ≤ p−2 (x ≠ 1).");
    }
}
