using System;
using System.Collections.Generic;

namespace CoenM.Z85e
{
    public static class Z85Extended
    {
        public static IEnumerable<byte> Decode(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var len = (uint)input.Length;
            var rest = len % 5;

            //  Accepts only strings bounded to 5 bytes
            if (len % 5 == 0)
                return Z85.Decode(input);

            var decodedSize = CalculateByteSize(input); //len * 4 / 5;
            var decoded = new byte[decodedSize];

            uint byteNbr = 0;
            uint charNbr = 0;
            uint value = 0;

            while (charNbr < len)
            {
                //  Accumulate value in base 85
                var c = input[(int)charNbr++];
                var index0 = (byte)c - 32;
                var b = Z85.Decoder[index0];
                value = value * 85 + b;

                //                value = value * 85 + Decoder[(byte)input[(int)charNbr++] - 32];

                if (charNbr % 5 == 0)
                {
                    //  Output value in base 256
                    uint divisor = 256 * 256 * 256;
                    while (divisor != 0)
                    {
                        decoded[byteNbr++] = (byte)(value / divisor % 256);
                        divisor /= 256;
                    }
                    value = 0;
                }
            }


            // take care of the rest.
            uint divisor2 = (uint)Math.Pow(256, rest-1-1); //todo check rest or rest + 1
            while (divisor2 != 0)
            {
                decoded[byteNbr++] = (byte)(value / divisor2 % 256);
                divisor2 /= 256;
            }

            return decoded;
        }


        private static int CalculateStringSize(byte[] data)
        {
            var size = (uint)data.Length;
            var rest = size % 4;

            if (rest == 0)
                return (int) (size * 5 / 4);

            var encodedSize = (size - rest)  * 5 / 4 + rest + 1;
            return (int) encodedSize;
        }


        private static int CalculateByteSize(string input)
        {
            var len = (uint)input.Length;
            var rest = len % 5;

            if (rest == 0)
                return  (int) (len * 4 / 5);

            var len2 = len - rest;
            return (int) (len2 * 4 / 5 + (rest - 1));
        }

        public static string Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var size = (uint)data.Length;

            if (size % 4 == 0)
                return Z85.Encode(data);


            var encodedSize = CalculateStringSize(data);
            var rest = size % 4;
            
            var encoded = new char[encodedSize];
            uint charNbr = 0;
            uint byteNbr = 0;
            uint value = 0;
            uint divisor;

            while (byteNbr < size)
            {
                //  Accumulate value in base 256 (binary)
                value = value * 256 + data[byteNbr++];
                if (byteNbr % 4 == 0)
                {
                    //  Output value in base 85
                    divisor = 85 * 85 * 85 * 85;
                    while (divisor != 0)
                    {
                        encoded[charNbr++] = Z85.Encoder[value / divisor % 85];
                        divisor /= 85;
                    }
                    value = 0;
                }
            }


            // take care of the rest.
            divisor = (uint) Math.Pow(85, rest);

            while (divisor != 0)
            {
                encoded[charNbr++] = Z85.Encoder[value / divisor % 85];
                divisor /= 85;
            }

            return new string(encoded);
        }
    }
}