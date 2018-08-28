using System;

namespace CoenM.Encoding.Test.TestData
{
    public class DecodeData
    {
        public DecodeData(string encodedString, byte[] expectedData, int expectedCharactersConsumed, int expectedBytesWritten)
        {
            EncodedString = encodedString.AsMemory();
            ExpectedData = expectedData;
            ExpectedCharactersConsumed = expectedCharactersConsumed;
            ExpectedBytesWritten = expectedBytesWritten;
        }

        public ReadOnlyMemory<char> EncodedString { get; }

        public byte[] ExpectedData { get; }

        public int ExpectedCharactersConsumed { get; }

        public int ExpectedBytesWritten { get; }
    }
}