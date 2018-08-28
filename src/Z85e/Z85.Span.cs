namespace CoenM.Encoding
{
#if FEATURE_SPAN

    using JetBrains.Annotations;
    using System;
    using System.Buffers;

    using Internals;

    public static partial class Z85
    {
        /// <summary>
        /// Decode the span of UTF-8 encoded text represented as Z85 into binary data.
        /// If the input is not a multiple of 5, it will decode as much as it can, to the closest multiple of 5.
        /// </summary>
        /// <param name="source">The input span which contains UTF-8 encoded text in Z85 that needs to be decoded.</param>
        /// <param name="destination">The output span which contains the result of the operation, i.e. the decoded binary data.</param>
        /// <param name="charsConsumed">The number of input characters consumed during the operation. This can be used to slice the input for subsequent calls, if necessary.</param>
        /// <param name="bytesWritten">The number of bytes written into the output span. This can be used to slice the output for subsequent calls, if necessary.</param>
        /// <param name="isFinalBlock">True (default) when the input span contains the entire data to decode.
        /// Set to false only if it is known that the input span contains partial data with more data to follow.</param>
        /// <returns>It returns the OperationStatus enum values:
        /// - Done - on successful processing of the entire input span
        /// - DestinationTooSmall - if there is not enough space in the output span to fit the decoded input
        /// - NeedMoreData - only if isFinalBlock is false and the input is not a multiple of 5, otherwise the partial input would be considered as InvalidData
        /// - InvalidData - if the input contains bytes outside of the expected Z85 range, or if it contains invalid/more than two padding characters,
        ///   or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.</returns>
        [PublicAPI]
        public static OperationStatus Decode(ReadOnlySpan<char> source, Span<byte> destination, out int charsConsumed, out int bytesWritten, bool isFinalBlock = true)
        {
            int srcLength = source.Length;
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (source.Length == 0)
                goto DoneExit;

            if (destination.Length < CalculateDecodedSize(source))
            {
                charsConsumed = 0;
                bytesWritten = 0;
                return OperationStatus.DestinationTooSmall;
            }

            var result = Decode(source.ToString());
            result.AsSpan().CopyTo(destination);

            charsConsumed = source.Length;
            bytesWritten = result.Length;
            return OperationStatus.Done;



            DoneExit:
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.Done;

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="bytesConsumed"></param>
        /// <param name="charsWritten"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Encode(ReadOnlySpan<byte> source, Span<char> destination, out int bytesConsumed, out int charsWritten)
        {
            if (destination.Length < CalculateEncodedSize(source))
            {
                bytesConsumed = 0;
                charsWritten = 0;
                return OperationStatus.DestinationTooSmall;
            }

            var result = Encode(source.ToArray());
            result.AsSpan().CopyTo(destination);

            bytesConsumed = source.Length;
            charsWritten = result.Length;
            return OperationStatus.Done;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalculateEncodedSize([NotNull] ReadOnlySpan<byte> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Z85Size.CalculateEncodedSize(source.Length);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalculateDecodedSize([NotNull] ReadOnlySpan<char> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Z85Size.CalculateDecodedSize(source.Length);
        }
    }
#else
    public static partial class Z85
    {
    }
#endif
}