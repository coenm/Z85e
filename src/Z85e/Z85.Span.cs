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
        /// <param name="mode"></param>
        /// <param name="isFinalBlock">True (default) when the input span contains the entire data to decode.
        /// Set to false only if it is known that the input span contains partial data with more data to follow.</param>
        /// <returns>It returns the OperationStatus enum values:
        /// - Done - on successful processing of the entire input span
        /// - DestinationTooSmall - if there is not enough space in the output span to fit the decoded input
        /// - NeedMoreData - only if isFinalBlock is false and the input is not a multiple of 5, otherwise the partial input would be considered as InvalidData
        /// - InvalidData - if the input contains bytes outside of the expected Z85 range, or if it contains invalid/more than two padding characters,
        ///   or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.</returns>
        [PublicAPI]
        public static OperationStatus Decode(
            ReadOnlySpan<char> source,
            Span<byte> destination,
            out int charsConsumed,
            out int bytesWritten,
            Z85Mode mode = Z85Mode.Padding,
            bool isFinalBlock = true)
        {
            int srcLength = source.Length;
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (source.Length == 0)
            {
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.Done;
            }

            var remainder = source.Length % 5;

            if (remainder > 0)
            {
                var usableSourceLength = source.Length - remainder;

                var usableSource = source.Slice(0, usableSourceLength);
                if (destination.Length < CalculateDecodedSize(usableSource))
                {
                    charsConsumed = 0;
                    bytesWritten = 0;
                    return OperationStatus.DestinationTooSmall;
                }

                {
                    var result2 = Decode(usableSource.ToString());
                    result2.AsSpan().CopyTo(destination);
                    charsConsumed = usableSource.Length;
                    bytesWritten = result2.Length;

                    if (isFinalBlock)
                    {
                        if (mode == Z85Mode.Padding)
                        {
                            // two chars are decoded to one byte
                            // thee chars to two bytes
                            // four chars to three bytes.
                            // therefore, remainder of one byte should not be possible.
                            if (remainder == 1)
                                return OperationStatus.InvalidData;
                            else
                            {
                                var padding = Z85Extended.Decode(source.Slice(usableSourceLength, remainder).ToString());
                                if (padding.Length + bytesWritten > destLength)
                                {
                                    return OperationStatus.DestinationTooSmall;
                                }
                                else
                                {
                                    padding.AsSpan().CopyTo(destination.Slice(bytesWritten, padding.Length));
                                    charsConsumed += remainder;
                                    bytesWritten += padding.Length;
                                    return OperationStatus.Done;
                                }
                            }
                        }
                        else
                        {
                            // strict mode doesn't allow final block to be a non multiple of 5.
                            charsConsumed = usableSource.Length;
                            bytesWritten = result2.Length;
                            return OperationStatus.InvalidData;
                        }
                    }
                    else
                    {
                        charsConsumed = usableSource.Length;
                        bytesWritten = result2.Length;
                        return OperationStatus.NeedMoreData;
                    }
                }
            }


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
        }

        ///  <summary>
        ///
        ///  </summary>
        ///  <param name="source"></param>
        ///  <param name="destination"></param>
        ///  <param name="bytesConsumed"></param>
        ///  <param name="charsWritten"></param>
        /// <param name="mode"></param>
        /// <param name="isFinalBlock"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Encode(
            ReadOnlySpan<byte> source,
            Span<char> destination,
            out int bytesConsumed,
            out int charsWritten,
            Z85Mode mode = Z85Mode.Padding,
            bool isFinalBlock = true)
        {

            int srcLength = source.Length;
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (srcLength == 0)
            {
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.Done;
            }

            int srcLengthToUse = 0;
            int destLengthToUse = 0;

            var srcRemainder = srcLength % 4;
            if (mode == Z85Mode.Padding && isFinalBlock)
            {
                if (srcRemainder == 1)
                {
                    // two chars are decoded to one byte
                    // thee chars to two bytes
                    // four chars to three bytes.
                    // therefore, remainder of one byte should not be possible.
                    bytesConsumed = 0;
                    charsWritten = 0;
                    return OperationStatus.InvalidData;
                }

                srcLengthToUse = srcLength;
                destLengthToUse = srcLength / 4 * 5;
            }
            else
            {
                srcLengthToUse = srcLength - srcRemainder;

            }





            var inputByteSize = srcLength;
            var usableInputByteSize = srcLength;

            var encodedSize = inputByteSize / 5 * 4;
            var remainder = inputByteSize % 5;


            if (isFinalBlock)
            {
                if (mode == Z85Mode.Padding)
                {
                    if (remainder == 1)
                    {
                        bytesConsumed = 0;
                        charsWritten = 0;
                        return OperationStatus.InvalidData;
                    }

                    {
                        var extraBytes = remainder - 1;
                        encodedSize = (inputByteSize - extraBytes) * 4 / 5 + extraBytes;
                    }
                }
                else
                {
                    // strict
                    if (remainder != 0)
                    {
                        bytesConsumed = 0;
                        charsWritten = 0;
                        return OperationStatus.InvalidData;
                    }
                }
            }
            else
            {
                // not final part
                if (encodedSize == 0 && inputByteSize > 0)
                {
                    bytesConsumed = 0;
                    charsWritten = 0;
                    return OperationStatus.NeedMoreData;
                }
            }

            if (destination.Length < encodedSize)
            {
                bytesConsumed = 0;
                charsWritten = 0;
                return OperationStatus.DestinationTooSmall;
            }

            var result = Z85Extended.Encode(source.Slice(0, usableInputByteSize).ToArray());
            result.AsSpan().CopyTo(destination);

            bytesConsumed = source.Length;
            charsWritten = result.Length;
            if (remainder == 0)
                return OperationStatus.Done;
            return OperationStatus.NeedMoreData;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalculateEncodedSize(ReadOnlySpan<byte> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var size = (uint)source.Length;
            var remainder = size % 5;

            if (remainder == 0)
                return 0;

            // two chars are decoded to one byte
            // thee chars to two bytes
            // four chars to three bytes.
            // therefore, remainder of one byte should not be possible.
            if (remainder == 1)
                throw new ArgumentException("Input length % 5 cannot be 1.");

            var extraBytes = remainder - 1;
            var decodedSize = (int)((size - extraBytes) * 4 / 5 + extraBytes);


            return Z85Size.CalculateEncodedSize(source.Length);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalculateDecodedSize(ReadOnlySpan<char> source)
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