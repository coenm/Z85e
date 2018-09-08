using System;
using System.Buffers;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    public class EncodeInputData
    {
        private readonly int _destinationLength;

        public EncodeInputData(byte[] source, bool isFinalBlock, int destinationLength = -1)
        {
            _destinationLength = destinationLength;
            IsFinalBlock = isFinalBlock;
            Source = source;
        }

        public bool IsFinalBlock { get; }

        public Memory<byte> Source { get; }

        public Memory<char> CreateDestination()
        {
            if (_destinationLength == 0)
                return Memory<char>.Empty;

            if (_destinationLength > 0)
                return new char[_destinationLength];

            return new char[Source.Length * 2]; // should always be enough
        }
    }

    public class EncodeExpectedData
    {
        public EncodeExpectedData(OperationStatus expectedResult, int expectedBytesConsumed, int expectedCharsWritten, string expectedOutput)
        {
            ExpectedResult = expectedResult;
            ExpectedBytesConsumed = expectedBytesConsumed;
            ExpectedCharsWritten = expectedCharsWritten;
            ExpectedOutput = expectedOutput;
        }

        public OperationStatus ExpectedResult { get; }

        public int ExpectedBytesConsumed { get; }

        public int ExpectedCharsWritten { get; }

        public string ExpectedOutput { get; }

        public void AssertResult(OperationStatus result, int bytesConsumed, int charsWritten, ReadOnlySpan<char> destination)
        {
            Assert.Equal(ExpectedResult, result);
            Assert.Equal(ExpectedBytesConsumed, bytesConsumed);
            Assert.Equal(ExpectedCharsWritten, charsWritten);
            Assert.Equal(ExpectedOutput, destination.Slice(0, charsWritten).ToString());
        }
    }
}