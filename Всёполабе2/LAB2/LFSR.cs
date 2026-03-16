using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LAB2
{
    public class LFSR
    {
        public LFSR(byte registerLength, string polynom)
        {
            RegisterLength = registerLength;
            _registerMask = Convert.ToInt64(new string('1', registerLength), 2);
            _xorBitsIndex = ParsePolynom(polynom);
        }

        private const string PolynomParsingPattern = "(?<=\\^)\\d+(?=\\+)";

        private readonly long _registerMask;
        private readonly List<byte> _xorBitsIndex;

        public byte RegisterLength { get; }


        public long RegisterState { get; private set; }

        public static string ParseInput(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Match m in Regex.Matches(input ?? string.Empty, "[01]+"))
            {
                sb.Append(m.Value);
            }
            return sb.ToString();
        }

        private List<byte> ParsePolynom(string polynom)
        {
            var bitsIndex = new List<byte>();
            foreach (Match match in Regex.Matches(polynom ?? string.Empty, PolynomParsingPattern))
            {
                bitsIndex.Add(Convert.ToByte(match.Value));
            }

            if (bitsIndex.Count == 0)
            {
                throw new ArgumentException("Неверный формат многочлена для LFSR.");
            }

            return bitsIndex;
        }

        public void SetRegisterState(string state)
        {
            string parsed = ParseInput(state);
            if (string.IsNullOrEmpty(parsed))
            {
                throw new ArgumentException("Начальное состояние должно содержать хотя бы один бит 0/1.");
            }

            if (parsed.Length > RegisterLength)
            {
                throw new ArgumentException($"Длина регистра превышена (максимум {RegisterLength} бит).");
            }

            RegisterState = Convert.ToInt64(parsed, 2);
        }


        public byte GetRandomByte()
        {
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                result <<= 1;
                result += Convert.ToByte((RegisterState >> (RegisterLength - 1)) & 1);

                byte nextBit = GetNextBit();
                RegisterState = (RegisterState << 1) & _registerMask;
                RegisterState += nextBit;
            }

            return result;
        }

        private byte GetNextBit()
        {
            byte nextBit = Convert.ToByte((RegisterState >> (_xorBitsIndex.First() - 1)) & 1);
            for (int i = 1; i < _xorBitsIndex.Count; i++)
            {
                nextBit ^= Convert.ToByte((RegisterState >> (_xorBitsIndex[i] - 1)) & 1);
            }
            return nextBit;
        }
    }
}

