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
        private const int MaximumEncodeLength = (int.MaxValue / 5) * 4;

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

//            var y = Unsafe.Add(ref dst, sourceIndex);

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
            return isFinalBlock ? OperationStatus.Done : OperationStatus.NeedMoreData;
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
        public static unsafe OperationStatus Encode(
            ReadOnlySpan<byte> source,
            Span<char> destination,
            out int bytesConsumed,
            out int charsWritten,
            bool isFinalBlock = true)
        {
            ref byte src = ref MemoryMarshal.GetReference(source);
            ref char dst = ref MemoryMarshal.GetReference(destination);
            ref char encoded = ref Map.Encoder[0];
            char* destAddress = (char*)Unsafe.AsPointer(ref dst);


            int srcLength = source.Length;
            int destLength = destination.Length;

            int sourceIndex = 0;
            int destIndex = 0;

            var maxSrcLength = srcLength;
            var requiredDestLength = srcLength / 4 * 5;
            if (requiredDestLength > destLength)
                maxSrcLength = destLength / 5 * 4; // trim down maxSrcLength
            maxSrcLength -= 4;

            while (sourceIndex <= maxSrcLength)
            {
                EncodeBlockSpan(ref Unsafe.Add(ref src, sourceIndex), destAddress + destIndex, ref encoded);
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
            if (endEncoded.Length <= (destLength - destIndex))
            {
                endEncoded.AsSpan().CopyTo(destination.Slice(destIndex));
                destIndex += srcLength - sourceIndex + 1;
                sourceIndex = srcLength;
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.Done;
            }
            else
            {
                bytesConsumed = sourceIndex;
                charsWritten = destIndex;
                return OperationStatus.DestinationTooSmall;
            }
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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void EncodeBlockSpan(ref byte sourceFourBytes, char* destination, ref char encodingMap)
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
            Unsafe.Write(destination + 0, Unsafe.Add(ref encodingMap, (int)(value / divisor4 % 85)));
            Unsafe.Write(destination + 1, Unsafe.Add(ref encodingMap, (int)(value / divisor3 % 85)));
            Unsafe.Write(destination + 2, Unsafe.Add(ref encodingMap, (int)(value / divisor2 % 85)));
            Unsafe.Write(destination + 3, Unsafe.Add(ref encodingMap, (int)(value / divisor1 % 85)));
            Unsafe.Write(destination + 4, Unsafe.Add(ref encodingMap, (int)(value % 85)));
        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        private static int EncodeBlock(ref byte sourceFourBytes, ref char destination, ref char encodingMap)
//        {
//            // Accumulate value in base 256 (binary)
//            var value = (sourceFourBytes << 24) | (Unsafe.Add(ref sourceFourBytes, 1) << 16) | (Unsafe.Add(ref sourceFourBytes, 2) << 8) | Unsafe.Add(ref sourceFourBytes, 3);
//            const uint divisor4 = 85 * 85 * 85 * 85;
////            const uint divisor3 = 85 * 85 * 85;
////            const uint divisor2 = 85 * 85;
////            const uint divisor1 = 85;
//
//            ref char add = ref Unsafe.Add(ref encodingMap, (int) (value / divisor4 % 85));
//            Unsafe.Write(ref Unsafe.Add(ref destination, 1), add);
////            Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 0), result);
////
////            destination[charNbr + 0] = z85Encoder[value / divisor4 % 85];
////            destination[charNbr + 1] = z85Encoder[value / divisor3 % 85];
////            destination[charNbr + 2] = z85Encoder[value / divisor2 % 85];
////            destination[charNbr + 3] = z85Encoder[value / divisor1 % 85];
////            destination[charNbr + 4] = z85Encoder[value % 85];
//
//
//            int i0 = Unsafe.Add(ref encodingMap, value >> 18);
//            int i1 = Unsafe.Add(ref encodingMap, (value >> 12) & 0x3F);
//            int i2 = Unsafe.Add(ref encodingMap, (value >> 6) & 0x3F);
//            int i3 = Unsafe.Add(ref encodingMap, value & 0x3F);
//
//            return i0 | (i1 << 8) | (i2 << 16) | (i3 << 24);
//        }
//
//        private static unsafe string EncodeInner([NotNull] byte[] data)
//        {
//            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
//            // ReSharper disable once HeuristicUnreachableCode
//            if (data == null)
//                return null;
//
//            var size = data.Length;
//            var encodedSize = Z85Size.CalculateEncodedSize(size);
//            var destination = new string('0', encodedSize);
//            int charNbr = 0;
//            int byteNbr = 0;
//
//            const uint divisor4 = 85 * 85 * 85 * 85;
//            const uint divisor3 = 85 * 85 * 85;
//            const uint divisor2 = 85 * 85;
//            const uint divisor1 = 85;
//            const int byte3 = 256 * 256 * 256;
//            const int byte2 = 256 * 256;
//            const int byte1 = 256;
//
//            // Get pointers to avoid unnecessary range checking
//            fixed (char* z85Encoder = Map.Encoder)
//            fixed (char* z85Dest = destination)
//            {
//                while (byteNbr < size)
//                {
//                    // Accumulate value in base 256 (binary)
//                    var value = (uint)(data[byteNbr + 0] * byte3 +
//                                       data[byteNbr + 1] * byte2 +
//                                       data[byteNbr + 2] * byte1 +
//                                       data[byteNbr + 3]);
//                    byteNbr += 4;
//
//                    //  Output value in base 85
//                    z85Dest[charNbr + 0] = z85Encoder[value / divisor4 % 85];
//                    z85Dest[charNbr + 1] = z85Encoder[value / divisor3 % 85];
//                    z85Dest[charNbr + 2] = z85Encoder[value / divisor2 % 85];
//                    z85Dest[charNbr + 3] = z85Encoder[value / divisor1 % 85];
//                    z85Dest[charNbr + 4] = z85Encoder[value % 85];
//                    charNbr += 5;
//                }
//            }
//
//            return destination;
//        }


    }
#else
    public static partial class Z85
    {
    }
#endif
}