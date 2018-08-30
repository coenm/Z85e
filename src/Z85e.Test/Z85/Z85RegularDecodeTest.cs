namespace CoenM.Encoding.Test.Z85
{
    using System;

    using TestData;

    using Xunit;

    using Sut = Encoding.Z85;

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