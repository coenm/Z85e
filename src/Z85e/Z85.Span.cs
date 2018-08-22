namespace CoenM.Encoding
{
#if FEATURE_SPAN

    using JetBrains.Annotations;
    using System;
    using System.Buffers;

    public static partial class Z85
    {
        /// <summary></summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="bytesConsumed"></param>
        /// <param name="charsWritten"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Decode(ReadOnlySpan<byte> source, Span<char> destination, out int bytesConsumed, out int charsWritten)
        {
            bytesConsumed = 0;
            charsWritten = 0;
            return OperationStatus.Done;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="charsConsumed"></param>
        /// <param name="bytesWritten"></param>
        /// <returns></returns>
        [PublicAPI]
        public static OperationStatus Encode(ReadOnlySpan<char> source, Span<byte> destination, out int charsConsumed, out int bytesWritten)
        {
            charsConsumed = 0;
            bytesWritten = 0;
            return OperationStatus.Done;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalcuateEncodedSize(ReadOnlySpan<byte> source)
        {
            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int CalcuateDecodedSize(ReadOnlySpan<char> source)
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