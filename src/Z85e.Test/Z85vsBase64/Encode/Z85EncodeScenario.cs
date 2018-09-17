using System;

namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    public class Z85EncodeScenario
    {
        public const int CHARS_FOR_ONE_BLOCK = 5;
        public const int BLOCK_SIZE = 4;

        public Z85EncodeScenario(byte[] source, bool isFinalBlock, int destinationLength = -1)
        {
            IsFinalBlock = isFinalBlock;
            Source = source;

            if (destinationLength <= -1)
                Destination = new char[source.Length * 2];
            else if (destinationLength == 0)
                Destination = Memory<char>.Empty;
            else
                Destination = new char[destinationLength];
        }

        public ReadOnlyMemory<byte> Source { get; }

        public Memory<char> Destination { get; }

        public bool IsFinalBlock { get; }

        public Z85Base64EncodeResult Encode()
        {
            var status = Encoding.Z85.Encode(
                Source.Span,
                Destination.Span,
                out var bytesConsumed,
                out var charsWritten,
                IsFinalBlock);

            return new Z85Base64EncodeResult(
                status,
                CalculateFullInputBlocks(bytesConsumed),
                AllBytesConsumed(bytesConsumed),
                bytesConsumed,
                CalculateFullOutputBlocks(charsWritten),
                charsWritten);
        }

        public override string ToString()
        {
            return $"{nameof(Source)} length: {Source.Length}{Environment.NewLine}{nameof(Destination)} length: {Destination.Length}{Environment.NewLine}{nameof(IsFinalBlock)}: {IsFinalBlock}";
        }

        private int CalculateFullInputBlocks(int bytesConsumed) => bytesConsumed / BLOCK_SIZE;

        private int CalculateFullOutputBlocks(int charsWritten) => charsWritten / CHARS_FOR_ONE_BLOCK;

        private bool AllBytesConsumed(int bytesConsumed) => bytesConsumed == Source.Length;
    }
}