using System;
using System.Collections.Generic;

namespace CoenM.Encoding
{
    /// <summary>
    /// Z85 Encoding library
    /// </summary>
    /// <remarks>This implementation is heavily based on https://github.com/zeromq/rfc/blob/master/src/spec_32.c </remarks>
    public static class Z85
    {
        //  Maps base 256 to base 85
        internal static readonly char[] Encoder =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', '.', '-', ':', '+', '=', '^', '!', '/',
            '*', '?', '&', '<', '>', '(', ')', '[', ']', '{',
            '}', '@', '%', '$', '#'
        };


        //  Maps base 85 to base 256
        //  We chop off lower 32 and higher 128 ranges
        internal static readonly byte[] Decoder =
        {
            0x00, 0x44, 0x00, 0x54, 0x53, 0x52, 0x48, 0x00,
            0x4B, 0x4C, 0x46, 0x41, 0x00, 0x3F, 0x3E, 0x45,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x40, 0x00, 0x49, 0x42, 0x4A, 0x47,
            0x51, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A,
            0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32,
            0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A,
            0x3B, 0x3C, 0x3D, 0x4D, 0x00, 0x4E, 0x43, 0x00,
            0x00, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
            0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
            0x21, 0x22, 0x23, 0x4F, 0x00, 0x50, 0x00, 0x00
        };

        /// <summary>
        /// Decode an encoded string into a byte array. Output size will be length of <paramref name="input"/> * 4 / 5.
        /// </summary>
        /// <remarks>This method will not check if <paramref name="input"/> only exists of Z85 characters.</remarks>
        /// <param name="input">encoded string. Should have length multiple of 5.</param>
        /// <returns><c>null</c> when input is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is not a multiple of 5.</exception>
        public static IEnumerable<byte> Decode(string input)
        {
            if (input == null)
                return null;

            var len = (uint)input.Length;

            //  Accepts only strings bounded to 5 bytes
            if (len % 5 != 0)
                throw new ArgumentOutOfRangeException(nameof(input), "Length of Input should be multiple of 5.");

            var decodedSize = len * 4 / 5;
            var decoded = new byte[decodedSize];

            uint byteNbr = 0;
            uint charNbr = 0;
            uint value = 0;

            while (charNbr < len)
            {
                //  Accumulate value in base 85
                value = value * 85 + Decoder[(byte)input[(int)charNbr++] - 32];
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
            return decoded;
        }

        /// <summary>
        /// Encode a byte array as a string. Output size will be length of <paramref name="data"/> / 4 * 5.
        /// </summary>
        /// <param name="data">byte[] to encode. Length should be multiple of 4.</param>
        /// <returns>Encoded string or <c>null</c> when the <paramref name="data"/> was null.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="data"/> is not a multiple of 4.</exception>
        public static string Encode(byte[] data)
        {
            if (data == null)
                return null;

            var size = (uint)data.Length;

            //  Accepts only byte arrays bounded to 4 bytes
            if (size % 4 != 0)
                throw new ArgumentOutOfRangeException(nameof(data), "Data length should be multiple of 4.");


            var encodedSize = size * 5 / 4;
            var encoded = new char[encodedSize];
            uint charNbr = 0;
            uint byteNbr = 0;
            uint value = 0;

            while (byteNbr < size)
            {
                //  Accumulate value in base 256 (binary)
                value = value * 256 + data[byteNbr++];
                if (byteNbr % 4 == 0)
                {
                    //  Output value in base 85
                    uint divisor = 85 * 85 * 85 * 85;
                    while (divisor != 0)
                    {
                        encoded[charNbr++] = Encoder[value / divisor % 85];
                        divisor /= 85;
                    }
                    value = 0;
                }
            }

            return new string(encoded);
        }
    }
}