using System;
using System.Collections.Generic;
using CoenM.Encoding.Internals;
using JetBrains.Annotations;

namespace CoenM.Encoding
{
    /// <summary>
    /// Z85 Encoding library
    /// </summary>
    /// <remarks>This implementation is heavily based on https://github.com/zeromq/rfc/blob/master/src/spec_32.c </remarks>
    public static class Z85
    {
        /// <summary>
        /// Decode an encoded string into a byte array. Output size will be length of <paramref name="input"/> * 4 / 5.
        /// </summary>
        /// <remarks>This method will not check if <paramref name="input"/> only exists of Z85 characters.</remarks>
        /// <param name="input">encoded string. Should have length multiple of 5.</param>
        /// <returns><c>null</c> when input is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is not a multiple of 5.</exception>
        public static IEnumerable<byte> Decode([NotNull] string input)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once HeuristicUnreachableCode
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
                value = value * 85 + Map.Decoder[(byte)input[(int)charNbr++] - 32];
                if (charNbr % 5 != 0)
                    continue;

                //  Output value in base 256
                uint divisor = 256 * 256 * 256;
                while (divisor != 0)
                {
                    decoded[byteNbr++] = (byte)(value / divisor % 256);
                    divisor /= 256;
                }
                value = 0;
            }
            return decoded;
        }

        /// <summary>
        /// Encode a byte array as a string. Output size will be length of <paramref name="data"/> / 4 * 5.
        /// </summary>
        /// <param name="data">byte[] to encode. Length should be multiple of 4.</param>
        /// <returns>Encoded string or <c>null</c> when the <paramref name="data"/> was null.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="data"/> is not a multiple of 4.</exception>
        // [System.Security.SecuritySafeCritical]
        public static unsafe string Encode(Span<byte> data)
        {
            var size = (uint)data.Length;

            //  Accepts only byte arrays bounded to 4 bytes
            if (size % 4 != 0)
                throw new ArgumentOutOfRangeException(nameof(data), "Data length should be multiple of 4.");


            var encodedSize = size * 5 / 4;
            Span<char> destination = new char[encodedSize];
            uint charNbr = 0;
            int byteNbr = 0;
            ReadOnlySpan<byte> dataSpan = data;

            const uint divisor4 = 85 * 85 * 85 * 85;
            const uint divisor3 = 85 * 85 * 85;
            const uint divisor2 = 85 * 85;
            const uint divisor1 = 85;
            const int byte3 = 256 * 256 * 256;
            const int byte2 = 256 * 256;
            const int byte1 = 256;

            // Get a pointer to the Map.Encoder table to avoid unnecessary range checking
            fixed (char* z85Encoder = Map.Encoder)
            {
                while (byteNbr < size)
                {
                    // Accumulate value in base 256 (binary)
                    ReadOnlySpan<byte> src = dataSpan.Slice(byteNbr, 4);
                    var value = (uint)(src[0] * byte3 + src[1] * byte2 + src[2] * byte1 + src[3]);
                    byteNbr += 4;

                    //  Output value in base 85
                    Span<char> dst = destination.Slice((int)charNbr, 5);
                    dst[0] = z85Encoder[value / divisor4 % 85];
                    dst[1] = z85Encoder[value / divisor3 % 85];
                    dst[2] = z85Encoder[value / divisor2 % 85];
                    dst[3] = z85Encoder[value / divisor1 % 85];
                    dst[4] = z85Encoder[value % 85];
                    charNbr += 5;
                }
            }

            return new string(destination.ToArray());
        }
    }
}