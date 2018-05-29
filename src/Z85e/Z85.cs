using System;

namespace CoenM.Encoding
{
    using Internals;

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
        /// <returns>empty bytes when <paramref name="input"/> is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is not a multiple of 5.</exception>
        public static ReadOnlySpan<byte> Decode(string input)
        {
            if (input == null)
                return null;

            var len = input.Length;

            //  Accepts only strings bounded to 5 bytes
            if (len % 5 != 0)
                throw new ArgumentOutOfRangeException(nameof(input), "Length of Input should be multiple of 5.");

            var decodedSize = len * 4 / 5;
            Span<byte> decoded = decodedSize <= 128
                ? stackalloc byte[decodedSize]
                : new byte[decodedSize];

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
                    decoded[(int) byteNbr++] = (byte)(value / divisor % 256);
                    divisor /= 256;
                }
                value = 0;
            }

            // todo: is this the way to do this?
            return decoded.ToArray();
        }

        /// <summary>
        /// Encode a byte array as a string. Output size will be length of <paramref name="data"/> / 4 * 5.
        /// </summary>
        /// <param name="data">byte[] to encode. Length should be multiple of 4.</param>
        /// <returns>Encoded string or <c>null</c> when the <paramref name="data"/> was null.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="data"/> is not a multiple of 4.</exception>
        public static string Encode(ReadOnlySpan<byte> data)
        {
            if (data == null)
                return null;

            var size = data.Length;

            //  Accepts only byte arrays bounded to 4 bytes
            if (size % 4 != 0)
                throw new ArgumentOutOfRangeException(nameof(data), "Data length should be multiple of 4.");


            var encodedSize = size * 5 / 4;
            Span<char> encoded = encodedSize <= 128
                ? stackalloc char[encodedSize]
                : new char[encodedSize];
            uint charNbr = 0;
            uint byteNbr = 0;
            uint value = 0;

            while (byteNbr < size)
            {
                //  Accumulate value in base 256 (binary)
                value = value * 256 + data[(int) byteNbr++];
                if (byteNbr % 4 != 0)
                    continue;

                //  Output value in base 85
                uint divisor = 85 * 85 * 85 * 85;
                while (divisor != 0)
                {
                    encoded[(int) charNbr++] = Map.Encoder[value / divisor % 85];
                    divisor /= 85;
                }
                value = 0;
            }

            // todo not sure if this is the way to do this.
            return new string(encoded.ToArray());
        }
    }
}