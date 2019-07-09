namespace CoenM.Encoding.Test.TestData
{
    using System;

    public class EncodeInputData
    {
        public EncodeInputData(byte[] source, bool isFinalBlock, int destinationLength = -1)
        {
            DestinationLength = destinationLength;
            IsFinalBlock = isFinalBlock;
            Source = source;
        }

        public int DestinationLength { get; }

        public bool IsFinalBlock { get; }

        public Memory<byte> Source { get; }

        public Memory<char> CreateDestination()
        {
            if (DestinationLength == 0)
                return Memory<char>.Empty;

            if (DestinationLength > 0)
                return new char[DestinationLength];

            return new char[Source.Length * 2]; // should always be enough
        }
    }
}
