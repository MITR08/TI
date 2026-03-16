using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2
{

    public static class StreamCrypt
    {

        public static (string keyBinary, byte[] result, string resultBinary) CryptBinary(LFSR keyGenerator, byte[] input)
        {
            if (keyGenerator == null) throw new ArgumentNullException(nameof(keyGenerator));
            if (input == null) throw new ArgumentNullException(nameof(input));

            StringBuilder sbKey = new StringBuilder();
            StringBuilder sbRes = new StringBuilder();
            List<byte> outBytes = new List<byte>(input.Length);

            foreach (byte b in input)
            {
                byte k = keyGenerator.GetRandomByte();
                byte r = (byte)(b ^ k);
                outBytes.Add(r);

                sbKey.Append(Convert.ToString(k, 2).PadLeft(8, '0')).Append(' ');
                sbRes.Append(Convert.ToString(r, 2).PadLeft(8, '0')).Append(' ');
            }

            return (sbKey.ToString(), outBytes.ToArray(), sbRes.ToString());
        }
    }
}

