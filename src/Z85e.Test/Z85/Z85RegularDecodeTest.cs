namespace CoenM.Encoding.Test.Z85
{
    using System;

    using CoenM.Encoding.Test.TestData;
    using Xunit;

    using Sut = CoenM.Encoding.Z85;

    public class Z85RegularDecodeTest
    {
        [Theory]
        [ClassData(typeof(StrictZ85Samples))]
        public void DecodeTest(byte[] data, string encoded)
        {
            Assert.Equal(Sut.Decode(encoded), data);
        }

        [Fact]
        public void DecodeNullReturnsNullTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Null(Sut.Decode(null));
        }

        [Theory]
        [ClassData(typeof(Z85InvalidEncodedStrings))]
        public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(string encoded)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Decode(encoded));
        }
    }
}
