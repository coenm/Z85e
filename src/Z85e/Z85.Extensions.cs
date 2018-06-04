
namespace CoenM.Encoding
{
    using System;
    using Internals.Guards;

    /// <summary>
    /// Z85 Encoding library
    /// </summary>
    /// <remarks>This implementation is heavily based on https://github.com/zeromq/rfc/blob/master/src/spec_32.c </remarks>
    public static partial class Z85
    {
        /// <summary>Decode an encoded string into a byte array. Output size will be length of <paramref name="input"/> * 4 / 5.</summary>
        /// <remarks>This method will not check if <paramref name="input"/> only exists of Z85 characters.</remarks>
        /// <param name="input">encoded string. Should have length multiple of 5.</param>
        /// <returns>empty bytes when <paramref name="input"/> is null, otherwise bytes containing the decoded input string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="input"/> is not a multiple of 5.</exception>
        public static ReadOnlyMemory<byte> Decode(string input)
        {
            Guard.NotNull(input, nameof(input));

            var inputSpan = input.AsSpan();

            var decodedSize = CalcuateDecodedSize(inputSpan);

//            Span<byte> decoded = decodedSize <= 128
//                ? stackalloc byte[decodedSize]
//                : new byte[decodedSize];

            Memory<byte> decoded = new byte[decodedSize];

            var len = Decode(inputSpan, decoded.Span);

            return decoded.Slice(0, len);
        }


        /// <summary>Encode bytes as a string. Output size will be length of <paramref name="source"/> / 4 * 5. </summary>
        /// <param name="source">bytes to encode. Length should be multiple of 4.</param>
        /// <returns>Encoded string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 4.</exception>
        public static string EncodeMemory(ReadOnlyMemory<byte> source)
        {
            var encodedSize = CalcuateEncodedSize(source.Span);

#if NETCOREAPP2_1
            return string.Create(encodedSize, source, (stringSpan, src) => Encode(source.Span, stringSpan));
#else
            Span<char> encoded = encodedSize <= 128
                ? stackalloc char[encodedSize]
                : new char[encodedSize];

            var len = Encode(source.Span, encoded);
            return new string(encoded.Slice(0, len).ToArray());
#endif
        }


        /// <summary>Encode bytes as a string. Output size will be length of <paramref name="source"/> / 4 * 5. </summary>
        /// <param name="source">bytes to encode. Length should be multiple of 4.</param>
        /// <returns>Encoded string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of <paramref name="source"/> is not a multiple of 4.</exception>
        public static string EncodeSpan(ReadOnlySpan<byte> source)
        {
            var encodedSize = CalcuateEncodedSize(source);

#if NETCOREAPP2_1
            // Obviously, we cannot use a span as a Func argument.
            // does not work.
            return string.Create(encodedSize, source, (stringSpan, src.ToArray()) => Encode(source.Span, stringSpan));
#else
            Span<char> encoded = encodedSize <= 128
                ? stackalloc char[encodedSize]
                : new char[encodedSize];

            var len = Encode(source, encoded);
            return new string(encoded.Slice(0, len).ToArray());
#endif
        }
    }
}