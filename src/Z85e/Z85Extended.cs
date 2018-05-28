using System;

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
        /// <returns>empty bytes when <paramref name="input"/> is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is a multiple of 5 plus 1.</exception>
        public static ReadOnlySpan<byte> Decode(string input)
        {
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
            Span<byte> decoded = decodedSize <= 128
                ? stackalloc byte[decodedSize]
                : new byte[decodedSize];


            uint byteNbr = 0;
            uint charNbr = 0;
            uint value = 0;
            uint divisor;

            while (charNbr < size)
            {
                //  Accumulate value in base 85
                value = value * 85 + Z85.Decoder[(byte)input[(int)charNbr++] - 32];

                if (charNbr % 5 != 0)
                    continue;

                //  Output value in base 256
                divisor = 256 * 256 * 256;
                while (divisor != 0)
                {
                    decoded[(int) byteNbr++] = (byte)(value / divisor % 256);
                    divisor /= 256;
                }
                value = 0;
            }

            // Take care of the remainder.
            divisor = (uint)Math.Pow(256, extraBytes - 1);
            while (divisor != 0)
            {
                decoded[(int) byteNbr++] = (byte)(value / divisor % 256);
                divisor /= 256;
            }


            // todo: is this the way to do this?
            return decoded.ToArray();
        }

        /// <summary>
        /// Encode a byte array as a string. Output size will roughly be 'length of <paramref name="data"/>' / 4 * 5.
        /// </summary>
        /// <param name="data">byte[] to encode. No restrictions on the length.</param>
        /// <returns>Encoded string or <c>null</c> when the <paramref name="data"/> was null.</returns>
        public static string Encode(byte[] data)
        {
            if (data == null)
                return null;

            var size = (uint)data.Length;
            var remainder = size % 4;

            if (remainder == 0)
                return Z85.Encode(data);

            // one byte -> two chars
            // two bytes -> three chars
            // three byte -> four chars
            var extraChars = remainder + 1;

            var encodedSize = (int)(size - remainder) * 5 / 4 + extraChars;

            var encoded = new char[encodedSize];
            uint charNbr = 0;
            uint byteNbr = 0;
            uint value = 0;
            uint divisor;

            while (byteNbr < size)
            {
                //  Accumulate value in base 256 (binary)
                value = value * 256 + data[byteNbr++];

                if (byteNbr % 4 != 0)
                    continue;

                //  Output value in base 85
                divisor = 85 * 85 * 85 * 85;
                while (divisor != 0)
                {
                    encoded[charNbr++] = Z85.Encoder[value / divisor % 85];
                    divisor /= 85;
                }
                value = 0;
            }

            // Take care of the remainder.
            divisor = (uint) Math.Pow(85, remainder);
            while (divisor != 0)
            {
                encoded[charNbr++] = Z85.Encoder[value / divisor % 85];
                divisor /= 85;
            }

            return new string(encoded);
        }
    }
}