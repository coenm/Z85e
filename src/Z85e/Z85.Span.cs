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
        /// Decode the span of encoded text represented as Z85 into binary data.
        /// If the input is not a multiple of 5, it will decode as much as it can, to the closest multiple of 5.
        /// </summary>
        /// <param name="source">The input span which contains encoded text in Z85 that needs to be decoded.</param>
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
        ///   or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.
        /// </returns>
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

            var nrCharactersRemaining = srcLength - sourceIndex; // should be: [1 .. 5)
            if (nrCharactersRemaining == 1)
            {
                // two chars are decoded to one byte
                // thee chars to two bytes
                // four chars to three bytes.
                // therefore, remainder of one byte should not be possible.
                charsConsumed = sourceIndex;
                bytesWritten = destIndex;
                return OperationStatus.InvalidData;
            }

            if (destLength - destIndex >= nrCharactersRemaining - 1)
            {
                if (nrCharactersRemaining == 2)
                {
                    DecodePartialTwoCharsSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref decoder);
                    charsConsumed = sourceIndex + 2;
                    bytesWritten = destIndex + 1;
                    return OperationStatus.Done;
                }

                if (nrCharactersRemaining == 3)
                {
                    DecodePartialThreeCharsSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref decoder);
                    charsConsumed = sourceIndex + 3;
                    bytesWritten = destIndex + 2;
                    return OperationStatus.Done;
                }

                if (nrCharactersRemaining == 4)
                {
                    DecodePartialFourCharsSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref decoder);
                    charsConsumed = sourceIndex + 4;
                    bytesWritten = destIndex + 3;
                    return OperationStatus.Done;
                }
            }

            charsConsumed = sourceIndex;
            bytesWritten = destIndex;
            return OperationStatus.DestinationTooSmall;
        }


        /// <summary>
        /// Encode the span of binary data into text represented as Z85.
        /// </summary>
        /// <param name="source">The input span which contains binary data that needs to be encoded.</param>
        /// <param name="destination">The output span which contains the result of the operation, i.e. the encoded text in Z85.</param>
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
            ref char encoder = ref Map.Encoder[0];

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
                EncodeBlockSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref encoder);
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

            var nrBytesRemaining = srcLength - sourceIndex; // should be: [1 .. 4)
            if (destLength - destIndex >= nrBytesRemaining + 1)
            {
                if (nrBytesRemaining == 1)
                {
                    EncodePartialOneByteSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref encoder);
                    bytesConsumed = sourceIndex + 1;
                    charsWritten = destIndex + 2;
                    return OperationStatus.Done;
                }

                if (nrBytesRemaining == 2)
                {
                    EncodePartialTwoBytesSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref encoder);
                    bytesConsumed = sourceIndex + 2;
                    charsWritten = destIndex + 3;
                    return OperationStatus.Done;
                }

                if (nrBytesRemaining == 3)
                {
                    EncodePartialThreeBytesSpan(ref Unsafe.Add(ref src, sourceIndex), ref Unsafe.Add(ref dst, destIndex), ref encoder);
                    bytesConsumed = sourceIndex + 3;
                    charsWritten = destIndex + 4;
                    return OperationStatus.Done;
                }
            }

            bytesConsumed = sourceIndex;
            charsWritten = destIndex;
            return OperationStatus.DestinationTooSmall;
        }

        /// <summary>
        /// Calculate size required to decode the given source.
        /// </summary>
        /// <param name="source">Input to decode</param>
        /// <returns>Byte length required to decode the given source <paramref name="source"/></returns>
        [PublicAPI]
        public static int GetDecodedSize(ReadOnlySpan<char> source) => GetDecodedSize(source.Length);

        /// <summary>
        /// Calculate size required to decode an encoded Span of size <paramref name="sourceLength"/>.
        /// </summary>
        /// <param name="sourceLength">Input size to decode. Should be positive.</param>
        /// <returns>Byte length required to decode the given source length.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified <paramref name="sourceLength"/> is less then <c>0</c>.</exception>
        [PublicAPI]
        public static int GetDecodedSize(int sourceLength)
        {
            if (sourceLength < 0)
                throw new ArgumentOutOfRangeException(nameof(sourceLength));

            var remainder = sourceLength % 5;

            if (remainder == 0)
                return sourceLength / 5 * 4;

            return sourceLength / 5 * 4 + remainder - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EncodeBlockSpan(ref byte sourceFourBytes, ref char destination, ref char z85Encoder)
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
        private static void EncodePartialThreeBytesSpan(ref byte sourceThreeBytes, ref char destination, ref char z85Encoder)
        {
            const uint divisor3 = 85 * 85 * 85;
            const uint divisor2 = 85 * 85;
            const uint divisor1 = 85;

            var value = (uint)((sourceThreeBytes << 16) +
                               (Unsafe.Add(ref sourceThreeBytes, 1) << 8) +
                               (Unsafe.Add(ref sourceThreeBytes, 2) << 0));

            //  Output value in base 85
            Unsafe.Add(ref destination, 0) = Unsafe.Add(ref z85Encoder, (int)(value / divisor3 % 85));
            Unsafe.Add(ref destination, 1) = Unsafe.Add(ref z85Encoder, (int)(value / divisor2 % 85));
            Unsafe.Add(ref destination, 2) = Unsafe.Add(ref z85Encoder, (int)(value / divisor1 % 85));
            Unsafe.Add(ref destination, 3) = Unsafe.Add(ref z85Encoder, (int)(value % 85));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EncodePartialTwoBytesSpan(ref byte sourceTwoBytes, ref char destination, ref char z85Encoder)
        {
            const uint divisor2 = 85 * 85;
            const uint divisor1 = 85;

            var value = (uint)((sourceTwoBytes << 8) +
                               Unsafe.Add(ref sourceTwoBytes, 1));

            //  Output value in base 85
            Unsafe.Add(ref destination, 0) = Unsafe.Add(ref z85Encoder, (int)(value / divisor2 % 85));
            Unsafe.Add(ref destination, 1) = Unsafe.Add(ref z85Encoder, (int)(value / divisor1 % 85));
            Unsafe.Add(ref destination, 2) = Unsafe.Add(ref z85Encoder, (int)(value % 85));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EncodePartialOneByteSpan(ref byte sourceOneByte, ref char destination, ref char z85Encoder)
        {
            const uint divisor1 = 85;

            var value = (uint)sourceOneByte;

            //  Output value in base 85
            Unsafe.Add(ref destination, 0) = Unsafe.Add(ref z85Encoder, (int)(value / divisor1 % 85));
            Unsafe.Add(ref destination, 1) = Unsafe.Add(ref z85Encoder, (int)(value % 85));
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
            Unsafe.Add(ref destination, 2) = (byte)(value >> 8);
            Unsafe.Add(ref destination, 3) = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DecodePartialFourCharsSpan(ref char sourceFourChars, ref byte destination, ref byte z85Decoder)
        {
            //  Accumulate value in base 85
            uint value = Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFourChars, 0));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFourChars, 1));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFourChars, 2));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceFourChars, 3));

            //  Output value in base 256
            Unsafe.Add(ref destination, 0) = (byte)(value >> 16);
            Unsafe.Add(ref destination, 1) = (byte)(value >> 8);
            Unsafe.Add(ref destination, 2) = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DecodePartialThreeCharsSpan(ref char sourceThreeChars, ref byte destination, ref byte z85Decoder)
        {
            //  Accumulate value in base 85
            uint value = Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceThreeChars, 0));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceThreeChars, 1));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceThreeChars, 2));

            //  Output value in base 256
            Unsafe.Add(ref destination, 0) = (byte)(value >> 8);
            Unsafe.Add(ref destination, 1) = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DecodePartialTwoCharsSpan(ref char sourceTwoChars, ref byte destination, ref byte z85Decoder)
        {
            //  Accumulate value in base 85
            uint value = Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceTwoChars, 0));
            value = value * 85 + Unsafe.Add(ref z85Decoder, Unsafe.Add(ref sourceTwoChars, 1));

            //  Output value in base 256
            Unsafe.Add(ref destination, 0) = (byte)value;
        }
    }
#endif
}