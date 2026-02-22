using System;
using System.Collections.Generic;
using System.Text;

namespace TESTLAB1
{
    /// <summary>Один шаг: входной символ, символ ключа (прогрессивного), результат.</summary>
    public class VigenereStep
    {
        public int Index { get; set; }
        public char InputChar { get; set; }
        public char KeyChar { get; set; }
        public char OutputChar { get; set; }
    }

    /// <summary>
    /// Шифр Виженера с прогрессивным ключом. Русский алфавит.
    /// Ключ повторяется с сдвигом: 1-й раз как есть, 2-й раз каждый символ +1 в алфавите, 3-й раз +2 и т.д.
    /// </summary>
    public class VigenereCipher
    {
        private const string Alphabet = Alphabets.Russian;
        private static readonly int N = Alphabet.Length;

        private readonly string _key;

        public VigenereCipher(string key)
        {
            _key = Alphabets.SanitizeKey(key, Alphabet);
            if (string.IsNullOrEmpty(_key))
                throw new ArgumentException("Ключ должен содержать хотя бы одну букву русского алфавита.");
        }

        /// <summary>
        /// Возвращает расширенный прогрессивный ключ для заданной длины текста.
        /// Показывает, во что преобразуется исходный ключ при повторениях (+0, +1, +2, ... в алфавите).
        /// </summary>
        public string GetProgressiveKeyExpansion(int length)
        {
            if (length <= 0) return "";
            var sb = new StringBuilder(length);
            int keyLen = _key.Length;
            for (int i = 0; i < length; i++)
            {
                int keyIndex = i % keyLen;
                int shift = i / keyLen;
                int ki = Alphabets.IndexInAlphabet(_key[keyIndex], Alphabet);
                int keyCharIndex = (ki + shift) % N;
                sb.Append(Alphabet[keyCharIndex]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Пошаговые данные для шифрования или расшифрования.
        /// </summary>
        public List<VigenereStep> GetSteps(string text, bool encrypt)
        {
            string letters = Alphabets.FilterToAlphabet(text ?? "", Alphabet);
            if (string.IsNullOrEmpty(letters)) return new List<VigenereStep>();
            var steps = new List<VigenereStep>();
            int keyLen = _key.Length;
            string keyStream = GetProgressiveKeyExpansion(letters.Length);
            for (int i = 0; i < letters.Length; i++)
            {
                int pi = Alphabets.IndexInAlphabet(letters[i], Alphabet);
                int ki = Alphabets.IndexInAlphabet(keyStream[i], Alphabet);
                int ci = encrypt ? (pi + ki) % N : (pi - ki + N) % N;
                steps.Add(new VigenereStep
                {
                    Index = i + 1,
                    InputChar = letters[i],
                    KeyChar = keyStream[i],
                    OutputChar = Alphabet[ci]
                });
            }
            return steps;
        }

        /// <summary>
        /// Зашифровать: только буквы русского алфавита, остальное игнорируется. Результат — только буквы.
        /// </summary>
        public string Encrypt(string plainText)
        {
            string letters = Alphabets.FilterToAlphabet(plainText ?? "", Alphabet);
            return Transform(letters, true);
        }

        /// <summary>
        /// Расшифровать.
        /// </summary>
        public string Decrypt(string cipherText)
        {
            string letters = Alphabets.FilterToAlphabet(cipherText ?? "", Alphabet);
            return Transform(letters, false);
        }

        private string Transform(string text, bool encrypt)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var sb = new StringBuilder(text.Length);
            int keyLen = _key.Length;
            for (int i = 0; i < text.Length; i++)
            {
                int keyIndex = i % keyLen;
                int shift = i / keyLen; // прогрессия: 0, 0, ..., 0, 1, 1, ..., 1, ...
                int ki = Alphabets.IndexInAlphabet(_key[keyIndex], Alphabet);
                if (ki < 0) continue;
                int keyCharIndex = (ki + shift) % N; // прогрессивный ключ
                int pi = Alphabets.IndexInAlphabet(text[i], Alphabet);
                if (pi < 0) continue;
                int ci = encrypt
                    ? (pi + keyCharIndex) % N
                    : (pi - keyCharIndex + N) % N;
                sb.Append(Alphabet[ci]);
            }
            return sb.ToString();
        }
    }
}
