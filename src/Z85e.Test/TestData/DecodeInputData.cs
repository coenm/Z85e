namespace CoenM.Encoding.Test.TestData
{
    using System;

    public class DecodeInputData
    {
        private readonly int destinationLength;

        public DecodeInputData(string source, bool isFinalBlock, int destinationLength = -1)
        {
            this.destinationLength = destinationLength;
            IsFinalBlock = isFinalBlock;
            Source = source.ToCharArray();
        }

        public bool IsFinalBlock { get; }

        public Memory<char> Source { get; }

        public Memory<byte> CreateDestination()
        {
            if (destinationLength == 0)
                return Memory<byte>.Empty;

            if (destinationLength > 0)
                return new byte[destinationLength];

            return new byte[Source.Length]; // should always be enough
        }
    }
}
