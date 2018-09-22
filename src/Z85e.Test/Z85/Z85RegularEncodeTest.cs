namespace CoenM.Encoding.Test.Z85
{
    using System;

    using TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85RegularEncodeTest
    {
        [Fact]
        public void HelloWorldEncodeTest()
        {
            Assert.Equal(Sut.Encode(StrictZ85Samples.HelloWorldData), StrictZ85Samples.HelloWorldEncoded);
        }

        [Theory]
        [ClassData(typeof(Z85InvalidDataLengths))]
        public void EncodeThrowsExceptionWhenInputHasWrongSizeTest(int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Encode(new byte[length]));
        }

        [Fact]
        public void DecodeNullReturnsNullTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Null(Sut.Decode(null));
        }

        [Fact]
        public void EncodeNullReturnsNullTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Null(Sut.Encode(null));
        }

        [Fact]
        public void MultipleEncodedStringsDecodeToSameBytes()
        {
            // arrange
            const string ENCODED1 = "00000";
            const string ENCODED2 = "%nSc1";

            // act
            var result1 = Sut.Decode(ENCODED1);
            var result2 = Sut.Decode(ENCODED2);

            // assert
            Assert.Equal(result1, result2);
        }
    }
}