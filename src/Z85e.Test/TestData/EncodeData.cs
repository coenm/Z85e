using System;
using System.Buffers;

namespace CoenM.Encoding.Test.TestData
{
    public class EncodeData
    {
        public EncodeData(byte[] data, string expectedEncodedString, int expectedBytesConsumed, int expectedCharsWritten, OperationStatus expectedResult = OperationStatus.Done)
        {
            Data = data.AsMemory();
            ExpectedEncodedString = expectedEncodedString;
            ExpectedBytesConsumed = expectedBytesConsumed;
            ExpectedCharsWritten = expectedCharsWritten;
            ExpectedResult = expectedResult;
        }

        public ReadOnlyMemory<byte> Data { get; }

        public string ExpectedEncodedString { get; }

        public int ExpectedBytesConsumed { get; }

        public int ExpectedCharsWritten { get; }

        public OperationStatus ExpectedResult { get; }

        public void Assert(OperationStatus result, int bytesConsumed, int charsWritten, ReadOnlySpan<char> encodedResult)
        {
            Xunit.Assert.Equal(ExpectedResult, result);
            Xunit.Assert.Equal(ExpectedBytesConsumed, bytesConsumed);
            Xunit.Assert.Equal(ExpectedCharsWritten, charsWritten);
            Xunit.Assert.Equal(ExpectedEncodedString, encodedResult.Slice(0, charsWritten).ToString());
        }
    }
}