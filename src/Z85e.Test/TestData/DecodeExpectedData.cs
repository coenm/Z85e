namespace CoenM.Encoding.Test.TestData
{
    using System;
    using System.Buffers;

    using Xunit;

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
