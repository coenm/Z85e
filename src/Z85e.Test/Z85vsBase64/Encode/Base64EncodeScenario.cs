using System;

namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    public class Base64EncodeScenario
    {
        public const int CHARS_FOR_ONE_BLOCK = 4;
        public const int BLOCK_SIZE = 3;

        public Base64EncodeScenario(byte[] source, bool isFinalBlock, int destinationLength = -1)
        {
            IsFinalBlock = isFinalBlock;

            Source = source;

            if (destinationLength <= -1)
                Destination = new byte[source.Length * 2];
            else if (destinationLength == 0)
                Destination = Memory<byte>.Empty;
            else
                Destination = new byte[destinationLength];
        }

        public bool IsFinalBlock { get; }

        public Memory<byte> Source { get; }

        public Memory<byte> Destination { get; }


        public Z85Base64EncodeResult Encode()
        {
            var status = System.Buffers.Text.Base64.EncodeToUtf8(
                Source.Span,
                Destination.Span,
                out var bytesConsumed,
                out var bytesWritten,
                IsFinalBlock);

            return new Z85Base64EncodeResult(
                status,
                CalculateFullInputBlocks(bytesConsumed),
                AllBytesConsumed(bytesConsumed),
                CalculateFullOutputBlocks(bytesWritten));
        }

        private int CalculateFullInputBlocks(int bytesConsumed) => bytesConsumed / BLOCK_SIZE;

        private int CalculateFullOutputBlocks(int bytesWritten) => bytesWritten / CHARS_FOR_ONE_BLOCK;

        private bool AllBytesConsumed(int bytesConsumed) => bytesConsumed == Source.Length;
    }
}