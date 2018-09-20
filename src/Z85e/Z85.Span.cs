namespace CoenM.Encoding
{
#if FEATURE_SPAN

    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
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
        public static OperationStatus Decode(
            ReadOnlySpan<char> source,
            Span<byte> destination,
            out int charsConsumed,
            out int bytesWritten,
            bool isFinalBlock = true)
        {
            ref char src = ref MemoryMarshal.GetReference(source);
            ref byte dst = ref MemoryMarshal.GetReference(destination);
            ref byte decoder = ref Map.Decoder[0];

            int srcLength = source.Length;
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            if (srcLength == 0)
            {
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.Done;
            }

            var maxSrcLength = srcLength;
            var requiredDestLength = srcLength / 5 * 4;
            if (requiredDestLength > destLength)
                maxSrcLength = destLength / 4 * 5; // trim down maxSrcLength

            maxSrcLength -= 5;

            while (sourceIndex <= maxSrcLength)
            {
                DecodeBlockSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref decoder);
                sourceIndex += 5;
                destIndex += 4;
            }

            if (destLength - destIndex < 4 && srcLength - sourceIndex >= 5)
            {
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.DestinationTooSmall;
            }

            if (!isFinalBlock)
            {
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.NeedMoreData;
            }

            if (srcLength == sourceIndex)
            {
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.Done;
            }

            var remainder = srcLength - sourceIndex; // should be 1 <= remainder < 5
            if (remainder == 1)
            {
                // two chars are decoded to one byte
                // thee chars to two bytes
                // four chars to three bytes.
                // therefore, remainder of one byte should not be possible.
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.InvalidData;
            }

            var endDecoded = Z85Extended.Decode(source.Slice(sourceIndex).ToString());
            if (endDecoded.Length <= destLength - destIndex)
            {
                endDecoded.AsSpan().CopyTo(destination.Slice(destIndex));
                destIndex += srcLength - sourceIndex - 1;
                sourceIndex = srcLength;
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.Done;
            }

            charsConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.DestinationTooSmall;
        }


        /// <summary>
        /// Encode the span of binary data into text represented as Z85.
        /// </summary>
        /// <param name="source">The input span which contains binary data that needs to be encoded.</param>
        /// <param name="destination">The output span which contains the result of the operation, i.e. the UTF-8 encoded text in Z85.</param>
        /// <param name="bytesConsumed">The number of input bytes consumed during the operation. This can be used to slice the input for subsequent calls, if necessary.</param>
        /// <param name="charsWritten">The number of characters written into the output span. This can be used to slice the output for subsequent calls, if necessary.</param>
        /// <param name="isFinalBlock"><c>True</c> (default) when the input span contains the entire data to encode.
        /// Set to false only if it is known that the input span contains partial data with more data to follow.</param>
        /// <returns>It returns the OperationStatus enum values:
        /// - Done - on successful processing of the entire input span
        /// - DestinationTooSmall - if there is not enough space in the output span to fit the encoded input
        /// - NeedMoreData - only if isFinalBlock is false, otherwise the output is padded if the input is not a multiple of 4
        /// It does not return InvalidData since that is not possible for Z85 encoding.</returns>
        [PublicAPI]
        public static OperationStatus Encode(
            ReadOnlySpan<byte> source,
            Span<char> destination,
            out int bytesConsumed,
            out int charsWritten,
            bool isFinalBlock = true)
        {
            ref byte src = ref MemoryMarshal.GetReference(source);
            ref char dst = ref MemoryMarshal.GetReference(destination);
            ref char encoded = ref Map.Encoder[0];

            var srcLength = source.Length;
            var destLength = destination.Length;

            var sourceIndex = 0;
            var destIndex = 0;

            var maxSrcLength = srcLength;
            var requiredDestLength = srcLength / 4 * 5;
            if (requiredDestLength > destLength)
                maxSrcLength = destLength / 5 * 4; // trim down maxSrcLength

            maxSrcLength -= 4;

            while (sourceIndex <= maxSrcLength)
            {
                EncodeBlockSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref encoded);
                sourceIndex += 4;
                destIndex += 5;
            }

            if (destLength - destIndex < 5 && srcLength - sourceIndex >= 4)
            {
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.DestinationTooSmall;
            }

            if (!isFinalBlock)
            {
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.NeedMoreData;
            }

            if (srcLength == sourceIndex)
            {
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.Done;
            }

            var endEncoded = Z85Extended.Encode(source.Slice(sourceIndex).ToArray());
            if (endEncoded.Length <= destLength - destIndex)
            {
                endEncoded.AsSpan().CopyTo(destination.Slice(destIndex));
                destIndex += srcLength - sourceIndex + 1;
                sourceIndex = srcLength;
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.Done;
            }

            bytesConsumed = sourceIndex;
            charsWritten = destIndex;
            return OperationStatus.DestinationTooSmall;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void EncodeBlockSpan(ref byte sourceFourBytes, ref char destination, ref char z85Encoder)
        {
            const uint divisor4 = 85 * 85 * 85 * 85;
            const uint divisor3 = 85 * 85 * 85;
            const uint divisor2 = 85 * 85;
            const uint divisor1 = 85;

            var value = (uint)((sourceFourBytes << 24) +
                               (Unsafe.Add(ref sourceFourBytes, 1) << 16) +
                               (Unsafe.Add(ref sourceFourBytes, 2) << 8) +
                               (Unsafe.Add(ref sourceFourBytes, 3) << 0));

            //  Output value in base 85
            Unsafe.Add(ref destination, 0) = Unsafe.Add(ref z85Encoder, (int)(value / divisor4 % 85));
            Unsafe.Add(ref destination, 1) = Unsafe.Add(ref z85Encoder, (int)(value / divisor3 % 85));
            Unsafe.Add(ref destination, 2) = Unsafe.Add(ref z85Encoder, (int)(value / divisor2 % 85));
            Unsafe.Add(ref destination, 3) = Unsafe.Add(ref z85Encoder, (int)(value / divisor1 % 85));
            Unsafe.Add(ref destination, 4) = Unsafe.Add(ref z85Encoder, (int)(value % 85));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DecodeBlockSpan(ref char sourceFiveChars, ref byte destination, ref byte z85Decoder)
        {
            //  Accumulate value in base 85
            uint value = Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFiveChars, 0));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFiveChars, 1));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFiveChars, 2));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFiveChars, 3));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFiveChars, 4));

            //  Output value in base 256
            Unsafe.Add(ref destination, 0) = (byte)(value >> 24);
            Unsafe.Add(ref destination, 1) = (byte)(value >> 16);
            Unsafe.Add(ref destination, 2) = (byte)(value >> 08);
            Unsafe.Add(ref destination, 3) = (byte)(value >> 00);
        }
    }
#endif
}