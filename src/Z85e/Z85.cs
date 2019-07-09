namespace CoenM.Encoding
{
    using System;

    using CoenM.Encoding.Internals;
    using JetBrains.Annotations;

    /// <summary>
    /// Z85 Encoding library.
    /// </summary>
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class Z85
    {
        /// <summary>
        /// Decode an encoded string into a byte array. Output size will be length of <paramref name="input"/> * 4 / 5.
        /// </summary>
        /// <remarks>This method will not check if <paramref name="input"/> only exists of Z85 characters.</remarks>
        /// <param name="input">encoded string. Should have length multiple of 5.</param>
        /// <returns><c>null</c> when input is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is not a multiple of 5.</exception>
        [PublicAPI]
        public static unsafe byte[] Decode([NotNull] string input)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once HeuristicUnreachableCode
            if (input == null)
                return null;

            var len = input.Length;
            var decodedSize = Z85Size.CalculateDecodedSize(len);
            var decoded = new byte[decodedSize];
            var byteNbr = 0;
            var charNbr = 0;

            const uint divisor3 = 256 * 256 * 256;
            const uint divisor2 = 256 * 256;
            const uint divisor1 = 256;

            // Get a pointers to avoid unnecessary range checking
            fixed (byte* z85Decoder = Map.Decoder)
            fixed (char* src = input)
            {
                while (charNbr < len)
                {
                    // Accumulate value in base 85
                    uint value = z85Decoder[(byte)src[charNbr]];
                    value = (value * 85) + z85Decoder[(byte)src[charNbr + 1]];
                    value = (value * 85) + z85Decoder[(byte)src[charNbr + 2]];
                    value = (value * 85) + z85Decoder[(byte)src[charNbr + 3]];
                    value = (value * 85) + z85Decoder[(byte)src[charNbr + 4]];
                    charNbr += 5;

                    // Output value in base 256
                    decoded[byteNbr + 0] = (byte)((value / divisor3) % 256);
                    decoded[byteNbr + 1] = (byte)((value / divisor2) % 256);
                    decoded[byteNbr + 2] = (byte)((value / divisor1) % 256);
                    decoded[byteNbr + 3] = (byte)(value % 256);
                    byteNbr += 4;
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
        // [System.Security.SecuritySafeCritical]
        [PublicAPI]
        public static unsafe string Encode([NotNull] byte[] data)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once HeuristicUnreachableCode
            if (data == null)
                return null;

            var size = data.Length;
            var encodedSize = Z85Size.CalculateEncodedSize(size);
            var destination = new string('0', encodedSize);
            int charNbr = 0;
            int byteNbr = 0;

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
                while (byteNbr < size)
                {
                    // Accumulate value in base 256 (binary)
                    var value = (uint)((data[byteNbr + 0] * byte3) +
                                       (data[byteNbr + 1] * byte2) +
                                       (data[byteNbr + 2] * byte1) +
                                       data[byteNbr + 3]);
                    byteNbr += 4;

                    // Output value in base 85
                    z85Dest[charNbr + 0] = z85Encoder[(value / divisor4) % 85];
                    z85Dest[charNbr + 1] = z85Encoder[(value / divisor3) % 85];
                    z85Dest[charNbr + 2] = z85Encoder[(value / divisor2) % 85];
                    z85Dest[charNbr + 3] = z85Encoder[(value / divisor1) % 85];
                    z85Dest[charNbr + 4] = z85Encoder[value % 85];
                    charNbr += 5;
                }
            }

            return destination;
        }
    }
}
