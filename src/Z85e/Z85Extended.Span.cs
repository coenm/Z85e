namespace CoenM.Encoding
{
#if FEATURE_SPAN

    using System;
    using System.Buffers;
    using JetBrains.Annotations;

    public static partial class Z85Extended
    {
        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="charsConsumed"></param>
        /// <param name="bytesWritten"></param>
        /// <param name="isFinalBlock"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Decode(ReadOnlySpan<char> source, Span<byte> destination, out int charsConsumed, out int bytesWritten, bool isFinalBlock)
        {
            var result = Decode(source.ToString());

            if (result.Length > destination.Length)
            {
                charsConsumed = 0;
                bytesWritten = 0;
                return OperationStatus.DestinationTooSmall;
            }

            result.AsSpan().CopyTo(destination);

            charsConsumed = source.Length;
            bytesWritten = result.Length;
            return OperationStatus.Done;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="bytesConsumed"></param>
        /// <param name="charsWritten"></param>
        /// <param name="isFinalBlock"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Encode(ReadOnlySpan<byte> source, Span<char> destination, out int bytesConsumed, out int charsWritten, bool isFinalBlock)
        {
            var result = Encode(source.ToArray());

            if (destination.Length < result.Length)
            {
                bytesConsumed = 0;
                charsWritten = 0;
                return OperationStatus.DestinationTooSmall;
            }

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
        public static int CalculateEncodedSize(ReadOnlySpan<byte> source)
        {
            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalculateDecodedSize(ReadOnlySpan<char> source)
        {
            return 0;
        }
    }
#else
    public static partial class Z85
    {
    }
#endif
}
