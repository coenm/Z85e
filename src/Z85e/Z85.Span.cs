namespace CoenM.Encoding
{
#if FEATURE_SPAN

    using JetBrains.Annotations;
    using System;
    using System.Buffers;

    using CoenM.Encoding.Internals;

    public static partial class Z85
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="charsConsumed"></param>
        /// <param name="bytesWritten"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Decode(ReadOnlySpan<char> source, Span<byte> destination, out int charsConsumed, out int bytesWritten)
        {
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