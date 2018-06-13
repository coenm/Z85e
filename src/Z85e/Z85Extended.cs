using System;
using System.Collections.Generic;
using CoenM.Encoding.Internals;
using JetBrains.Annotations;

namespace CoenM.Encoding
{
    /// <summary>
    /// Z85 Extended Encoding library. Z85 Extended doesn't require the length of the bytes to be a multiple of 4.
    /// </summary>
    public static class Z85Extended
    {
        /// <summary>
        /// Decode an encoded string into a byte array. Output size will roughly be 'length of <paramref name="input"/>' * 4 / 5.
        /// </summary>
        /// <remarks>This method will not check if <paramref name="input"/> only exists of Z85 characters.</remarks>
        /// <param name="input">encoded string.</param>
        /// <returns><c>null</c> when <paramref name="input"/> is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is a multiple of 5 plus 1.</exception>
        public static unsafe IEnumerable<byte> Decode([NotNull] string input)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once HeuristicUnreachableCode
            if (input == null)
                return null;

            var size = (uint)input.Length;
            var remainder = size % 5;

            if (remainder == 0)
                return Z85.Decode(input);

            // two chars are decoded to one byte
            // thee chars to two bytes
            // four chars to three bytes.
            // threfore, remainder of one byte should not be possible.
            if (remainder == 1)
                throw new ArgumentException("Input length % 5 cannot be 1.");

            var extraBytes = remainder - 1;
            var decodedSize = (int)((size - extraBytes) * 4 / 5 + extraBytes);

            var decoded = new byte[decodedSize];

            uint byteNbr = 0;
            uint charNbr = 0;
            uint value;

            const uint divisor3 = 256 * 256 * 256;
            const uint divisor2 = 256 * 256;
            const uint divisor1 = 256;

            var size2 = size - remainder;

            // Get a pointers to avoid unnecessary range checking
            fixed (byte* z85Decoder = Map.Decoder)
            fixed (char* src = input)
            {
                while (charNbr < size2)
                {
                    //  Accumulate value in base 85
                    value = z85Decoder[(byte)src[charNbr]];
                    value = value * 85 + z85Decoder[(byte)src[charNbr + 1]];
                    value = value * 85 + z85Decoder[(byte)src[charNbr + 2]];
                    value = value * 85 + z85Decoder[(byte)src[charNbr + 3]];
                    value = value * 85 + z85Decoder[(byte)src[charNbr + 4]];
                    charNbr += 5;

                    //  Output value in base 256
                    decoded[byteNbr + 0] = (byte)(value / divisor3 % 256);
                    decoded[byteNbr + 1] = (byte)(value / divisor2 % 256);
                    decoded[byteNbr + 2] = (byte)(value / divisor1 % 256);
                    decoded[byteNbr + 3] = (byte)(value % 256);
                    byteNbr += 4;
                }
            }

            value = 0;
            while (charNbr < size)
                value = value * 85 + Map.Decoder[(byte) input[(int) charNbr++]];

            // Take care of the remainder.
            var divisor = (uint)Math.Pow(256, extraBytes - 1);
            while (divisor != 0)
            {
                decoded[byteNbr++] = (byte)(value / divisor % 256);
                divisor /= 256;
            }

            return decoded;
        }

        /// <summary>
        /// Encode a byte array as a string. Output size will roughly be 'length of <paramref name="data"/>' / 4 * 5.
        /// </summary>
        /// <param name="data">byte[] to encode. No restrictions on the length.</param>
        /// <returns>Encoded string or <c>null</c> when the <paramref name="data"/> was null.</returns>
        public static unsafe string Encode([NotNull] byte[] data)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once HeuristicUnreachableCode
            if (data == null)
                return null;

            var size = data.Length;
            var remainder = size % 4;

            if (remainder == 0)
                return Z85.Encode(data);

            // one byte -> two chars
            // two bytes -> three chars
            // three byte -> four chars
            var extraChars = remainder + 1;

            var encodedSize = (size - remainder) * 5 / 4 + extraChars;
            var destination = new string('0', encodedSize);
            uint charNbr = 0;
            uint byteNbr = 0;

            var size2 = size - remainder;

            const uint divisor4 = 85 * 85 * 85 * 85;
            const uint divisor3 = 85 * 85 * 85;
            const uint divisor2 = 85 * 85;
            const uint divisor1 = 85;
            const int byte3 = 256 * 256 * 256;
            const int byte2 = 256 * 256;
            const int byte1 = 256;

            // Get pointers to avoid unnecessary range checking
            fixed (char* z85Encoder = Map.Encoder)
            fixed (char* z85Dest = destination)
            {
                uint value;
                while (byteNbr < size2)
                {
                    // Accumulate value in base 256 (binary)
                    value = (uint)(data[byteNbr + 0] * byte3 +
                                   data[byteNbr + 1] * byte2 +
                                   data[byteNbr + 2] * byte1 +
                                   data[byteNbr + 3]);
                    byteNbr += 4;

                    //  Output value in base 85
                    z85Dest[charNbr + 0] = z85Encoder[value / divisor4 % 85];
                    z85Dest[charNbr + 1] = z85Encoder[value / divisor3 % 85];
                    z85Dest[charNbr + 2] = z85Encoder[value / divisor2 % 85];
                    z85Dest[charNbr + 3] = z85Encoder[value / divisor1 % 85];
                    z85Dest[charNbr + 4] = z85Encoder[value % 85];
                    charNbr += 5;
                }


                // Take care of the remainder.
                value = 0;
                while (byteNbr < size)
                    value = value * 256 + data[byteNbr++];

                var divisor = (uint) Math.Pow(85, remainder);
                while (divisor != 0)
                {
                    z85Dest[charNbr++] = z85Encoder[value / divisor % 85];
                    divisor /= 85;
                }
            }

            return destination;
        }
    }
}