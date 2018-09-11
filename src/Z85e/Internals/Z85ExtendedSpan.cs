namespace CoenM.Encoding.Internals
{
#if FEATURE_SPAN

    using System;
    using System.Buffers;

    internal static class Z85ExtendedSpan
    {
        internal static OperationStatus Decode(
            ReadOnlySpan<char> source,
            Span<byte> destination,
            out int charsConsumed,
            out int bytesWritten,
            bool isFinalBlock = true)
        {
            bytesWritten = 0;
            charsConsumed = 0;
            return OperationStatus.DestinationTooSmall;
        }


        internal static OperationStatus Encode(
            ReadOnlySpan<byte> source,
            Span<char> destination,
            out int bytesConsumed,
            out int charsWritten,
            bool isFinalBlock = true)
        {
            var srcLen = source.Length;
            if (srcLen == 0)
            {
                bytesConsumed = 0;
                charsWritten = 0;
                return OperationStatus.Done;
            }

 
            if (isFinalBlock)
            {

            }
            else
            {

            }


            bytesConsumed = 0;
            charsWritten = 0;
            return OperationStatus.DestinationTooSmall;
        }
    }
#endif
}