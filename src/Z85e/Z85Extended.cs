using System;
using System.Diagnostics;
using CoenM.Encoding.Internals.Guards;

namespace CoenM.Encoding
{
    using Internals;

    /// <summary>
    /// Z85 Encoding library
    /// </summary>
    /// <remarks>This implementation is heavily based on https://github.com/zeromq/rfc/blob/master/src/spec_32.c </remarks>
    public static partial class Z85Extended
    {
        /// <summary>Calculate output size after decoding the z85 characters.</summary>
        /// <param name="source">encoded string</param>
        /// <returns>size of output after decoding</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 5.</exception>
        public static int CalcuateDecodedSize(ReadOnlySpan<char> source)
        {
            Guard.MustBeGreaterThanOrEqualTo(source.Length, 1, nameof(source));

            var size = (uint)source.Length;
            var remainder = size % 5;

            if (remainder == 0)
                return Z85.CalcuateDecodedSize(source);

            // two chars are decoded to one byte
            // thee chars to two bytes
            // four chars to three bytes.
            // threfore, remainder of one byte should not be possible.
            if (remainder == 1)
                throw new ArgumentException("Input length % 5 cannot be 1.");

            var extraBytes = remainder - 1;
            var decodedSize = (int)((size - extraBytes) * 4 / 5 + extraBytes);

            return decodedSize;

        }

        /// <summary>Calculate string size after encoding bytes using the Z85 encoder.</summary>
        /// <param name="source">bytes to encode</param>
        /// <returns>size of the encoded string</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 4.</exception>
        public static int CalcuateEncodedSize(ReadOnlySpan<byte> source)
        {
            Guard.MustBeGreaterThanOrEqualTo(source.Length, 1, nameof(source));

            var size = source.Length;
            var remainder = size % 4;

            if (remainder == 0)
                return Z85.CalcuateEncodedSize(source);

            // one byte -> two chars
            // two bytes -> three chars
            // three byte -> four chars
            var extraChars = remainder + 1;

            var encodedSize = (size - remainder) * 5 / 4 + extraChars;

            return encodedSize;
        }



        /// <summary>Decode an encoded string (<paramref name="source"/>) to bytes (<paramref name="destination"/>).</summary>
        /// <remarks>This method will not check if <paramref name="source"/> only exists of Z85 characters.</remarks>
        /// <param name="source">encoded string. Should have length multiple of 5.</param>
        /// <param name="destination">placeholder for the decoded result. Should have sufficient length.</param>
        /// <returns>number of bytes written to <paramref name="destination"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 5, or when destination doesn't have sufficient space.</exception>
        public static int Decode(ReadOnlySpan<char> source, Span<byte> destination)
        {
            var decodedSize = CalcuateDecodedSize(source);
            Guard.MustBeSizedAtLeast(destination, decodedSize, nameof(destination));

            var len = source.Length;

            var remainder = len % 5;

            if (remainder == 0)
                return Z85.Decode(source, destination);

            var extraBytes = remainder - 1;



            var byteNbr = 0;
            var charNbr = 0;
            uint value = 0;
            uint divisor;


            var firstPartLenChar = source.Length - remainder;
            var firstPartLenByte = Z85.CalcuateDecodedSize(source.Slice(0, firstPartLenChar));

            byteNbr = Z85.Decode(source.Slice(0, firstPartLenChar), destination);
            Debug.Assert(byteNbr == firstPartLenChar / 5 * 4, "byteNbr == firstPartLenChar");
            Debug.Assert(value == 0, "Value should be 0");
            Debug.Assert(charNbr == firstPartLenChar, "charNbr should be firstPartLenChar");

            charNbr = firstPartLenChar;



            // remaining.
            // then last part
            while (charNbr < len)
            {
                //  Accumulate value in base 85
                value = value * 85 + Map.Decoder[(byte)source[charNbr++] - 32];

                if (charNbr % 5 != 0)
                    continue;

                throw new Exception();
            }

            // Take care of the remainder.
            divisor = (uint)Math.Pow(256, extraBytes - 1);
            while (divisor != 0)
            {
                destination[byteNbr++] = (byte)(value / divisor % 256);
                divisor /= 256;
            }

            return decodedSize;
        }


        /// <summary>Encode bytes (<paramref name="source"/>) to characters (<paramref name="destination"/>).</summary>
        /// <param name="source">bytes to encode. Length should be multiple of 4.</param>
        /// <param name="destination">placeholder for the ecoded result. Should have sufficient length.</param>
        /// <returns>number of characters written to <paramref name="destination"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 4, or when destination doesn't have sufficient space.</exception>
        public static int Encode(ReadOnlySpan<byte> source, Span<char> destination)
        {
            Guard.MustHaveSizeMultipleOf(source, 4, nameof(source));

            var encodedSize = CalcuateEncodedSize(source);
            Guard.MustBeSizedAtLeast(destination, encodedSize, nameof(destination));

            uint charNbr = 0;
            uint byteNbr = 0;
            uint value = 0;

            while (byteNbr < source.Length)
            {
                //  Accumulate value in base 256 (binary)
                value = value * 256 + source[(int)byteNbr++];
                if (byteNbr % 4 != 0)
                    continue;

                //  Output value in base 85
                uint divisor = 85 * 85 * 85 * 85;
                while (divisor != 0)
                {
                    destination[(int)charNbr++] = Map.Encoder[value / divisor % 85];
                    divisor /= 85;
                }
                value = 0;
            }

            return encodedSize;
        }
    }
}