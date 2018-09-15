using System;

namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    public class Z85DecodeScenario
    {
        public const int CHARS_FOR_ONE_BLOCK = 5;
        public const int BLOCK_SIZE = 4;

        public Z85DecodeScenario(string source, bool isFinalBlock, int destinationLength = -1)
        {
            IsFinalBlock = isFinalBlock;
            Source = source.AsMemory();

            if (destinationLength <= -1)
                Destination = new byte[source.Length * 2];
            else if (destinationLength == 0)
                Destination = Memory<byte>.Empty;
            else
                Destination = new byte[destinationLength];
        }

        public ReadOnlyMemory<char> Source { get; }

        public Memory<byte> Destination { get; }

        public bool IsFinalBlock { get; }

        public Z85Base64DecodeResult Decode()
        {
            var status = Encoding.Z85.Decode(
                Source.Span,
                Destination.Span,
                out var charsConsumed,
                out var bytesWritten,
                IsFinalBlock);

            return new Z85Base64DecodeResult(
                status,
                CalculateFullInputBlocks(charsConsumed),
                AllCharsConsumed(charsConsumed),
                CalculateFullOutputBlocks(bytesWritten));
        }

        private int CalculateFullInputBlocks(int charsConsumed) => charsConsumed / CHARS_FOR_ONE_BLOCK;

        private int CalculateFullOutputBlocks(int bytesWritten) => bytesWritten / BLOCK_SIZE;

        private bool AllCharsConsumed(int charsConsumed) => charsConsumed == Source.Length;


        public override string ToString()
        {
            return $"{nameof(Source)}: {Source.ToString()}{Environment.NewLine}{nameof(Destination)} length: {Destination.Length}{Environment.NewLine}{nameof(IsFinalBlock)}: {IsFinalBlock}";
        }
    }
}