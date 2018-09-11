using System;
using System.Buffers;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    public class DecodeInputData
    {
        private readonly int _destinationLength;

        public DecodeInputData(string source, bool isFinalBlock, int destinationLength = -1)
        {
            _destinationLength = destinationLength;
            IsFinalBlock = isFinalBlock;
            Source = source.ToCharArray();
        }

        public bool IsFinalBlock { get; }

        public Memory<char> Source { get; }

        public Memory<byte> CreateDestination()
        {
            if (_destinationLength == 0)
                return Memory<byte>.Empty;

            if (_destinationLength > 0)
                return new byte[_destinationLength];

            return new byte[Source.Length]; // should always be enough
        }
    }

    public class DecodeExpectedData
    {
        public DecodeExpectedData(OperationStatus expectedResult, int expectedCharsConsumed, int expectedBytesWritten, ReadOnlySpan<byte> expectedOutput)
        {
            ExpectedResult = expectedResult;
            ExpectedCharsConsumed = expectedCharsConsumed;
            ExpectedBytesWritten = expectedBytesWritten;
            ExpectedOutput = expectedOutput.ToArray();
        }

        public OperationStatus ExpectedResult { get; }

        public int ExpectedCharsConsumed { get; }

        public int ExpectedBytesWritten { get; }

        public byte[] ExpectedOutput { get; }

        public void AssertResult(OperationStatus result, int charsConsumed, int bytesWritten, ReadOnlySpan<byte> destination)
        {
            Assert.Equal(ExpectedResult, result);
            Assert.Equal(ExpectedCharsConsumed, charsConsumed);
            Assert.Equal(ExpectedBytesWritten, bytesWritten);
            Assert.Equal(ExpectedOutput, destination.Slice(0, bytesWritten).ToArray());
        }
    }
}