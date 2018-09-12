using System;

namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    public class Base64DecodeScenario
    {
        public const int CHARS_FOR_ONE_BLOCK = 4;
        public const int BLOCK_SIZE = 3;

        public Base64DecodeScenario(string source, bool isFinalBlock, int destinationLength = -1)
        {
            IsFinalBlock = isFinalBlock;

            Source = System.Text.Encoding.UTF8.GetBytes(source);

            if (destinationLength <= -1)
                Destination = new byte[source.Length * 2];
            else if (destinationLength == 0)
                Destination = Memory<byte>.Empty;
            else
                Destination = new byte[destinationLength];
        }

        public Base64DecodeScenario(byte[] source, bool isFinalBlock, int destinationLength = -1)
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


        public Z85Base64DecodeResult Decode()
        {
            var status = System.Buffers.Text.Base64.DecodeFromUtf8(
                Source.Span,
                Destination.Span,
                out var bytesConsumed,
                out var bytesWritten,
                IsFinalBlock);

            return new Z85Base64DecodeResult(
                status,
                CalculateFullInputBlocks(bytesConsumed),
                AllCharsConsumed(bytesConsumed),
                CalculateFullOutputBlocks(bytesWritten));
        }

        private int CalculateFullInputBlocks(int charsConsumed) => charsConsumed / CHARS_FOR_ONE_BLOCK;

        private int CalculateFullOutputBlocks(int bytesWritten) => bytesWritten / BLOCK_SIZE;

        private bool AllCharsConsumed(int charsConsumed) => charsConsumed == Source.Length;
    }
}