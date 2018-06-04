using System;
using CoenM.Encoding.Internals.Guards;

namespace CoenM.Encoding
{
    using Internals;

    /// <summary>
    /// Z85 Encoding library
    /// </summary>
    /// <remarks>This implementation is heavily based on https://github.com/zeromq/rfc/blob/master/src/spec_32.c </remarks>
    public static partial class Z85
    {
        /// <summary>Calculate output size after decoding the z85 characters.</summary>
        /// <param name="source">encoded string</param>
        /// <returns>size of output after decoding</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 5.</exception>
        public static int CalcuateDecodedSize(ReadOnlySpan<char> source)
        {
            Guard.MustHaveSizeMultipleOf(source, 5, nameof(source));

            return source.Length / 5 * 4;
        }

        /// <summary>Calculate string size after encoding bytes using the Z85 encoder.</summary>
        /// <param name="source">bytes to encode</param>
        /// <returns>size of the encoded string</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 4.</exception>
        public static int CalcuateEncodedSize(ReadOnlySpan<byte> source)
        {
            Guard.MustHaveSizeMultipleOf(source, 4, nameof(source));

            return source.Length / 4 * 5;
        }

        /// <summary>Decode an encoded string (<paramref name="source"/>) to bytes (<paramref name="destination"/>).</summary>
        /// <remarks>This method will not check if <paramref name="source"/> only exists of Z85 characters.</remarks>
        /// <param name="source">encoded string. Should have length multiple of 5.</param>
        /// <param name="destination">placeholder for the decoded result. Should have sufficient length.</param>
        /// <returns>number of bytes written to <paramref name="destination"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 5, or when destination doesn't have sufficient space.</exception>
        public static int Decode(ReadOnlySpan<char> source, Span<byte> destination)
        {
            Guard.MustHaveSizeMultipleOf(source, 5, nameof(source));

            var decodedSize = CalcuateDecodedSize(source);
            Guard.MustBeSizedAtLeast(destination, decodedSize, nameof(destination));

            var len = source.Length;
            var byteNbr = 0;
            var charNbr = 0;
            uint value = 0;

            while (charNbr < len)
            {
                //  Accumulate value in base 85
                value = value * 85 + Map.Decoder[(byte)source[charNbr++] - 32];
                if (charNbr % 5 != 0)
                    continue;

                //  Output value in base 256
                var divisor = 256 * 256 * 256;
                while (divisor != 0)
                {
                    destination[byteNbr++] = (byte)(value / divisor % 256);
                    divisor /= 256;
                }
                value = 0;
            }

            return decodedSize;
        }


        /// <summary>Encode bytes (<paramref name="source"/>) to characters (<paramref name="destination"/>).</summary>
        /// <param name="source">bytes to encode. Length should be multiple of 4.</param>
        /// <param name="destination">placeholder for the ecoded result. Should have sufficient length.</param>
        /// <returns>number of characters written to <paramref name="destination"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 4, or when destination doesn't have sufficient space.</exception>
        // [System.Security.SecuritySafeCritical]
        public static unsafe int Encode(ReadOnlySpan<byte> source, Span<char> destination)
        {
            Guard.MustHaveSizeMultipleOf(source, 4, nameof(source));

            var encodedSize = CalcuateEncodedSize(source);
            Guard.MustBeSizedAtLeast(destination, encodedSize, nameof(destination));

            uint charNbr = 0;
            int byteNbr = 0;
            var len = source.Length;

            const uint divisor4 = 85 * 85 * 85 * 85;
            const uint divisor3 = 85 * 85 * 85;
            const uint divisor2 = 85 * 85;
            const uint divisor1 = 85;
            const int byte3 = 256 * 256 * 256;
            const int byte2 = 256 * 256 ;
            const int byte1 = 256 ;

            // Get a pointer to the Map.Encoder table to avoid unnecessary range checking
            fixed (char* z85Encoder = Map.Encoder)
            {
                while (byteNbr < len)
                {
                    // Accumulate value in base 256 (binary)
                    ReadOnlySpan<byte> src = source.Slice(byteNbr, 4);
                    var value = (uint) (src[0] * byte3 + src[1] * byte2 + src[2] * byte1 + src[3]);
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

            return encodedSize;
        }
    }
}