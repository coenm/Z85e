namespace CoenM.Encoding.Test.TestData
{
    using System;
    using System.Buffers;

    using Xunit;

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
