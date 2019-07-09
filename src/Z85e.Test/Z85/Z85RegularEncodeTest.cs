namespace CoenM.Encoding.Test.Z85
{
    using System;

    using CoenM.Encoding.Test.TestData;
    using Xunit;

    using Sut = CoenM.Encoding.Z85;

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
            const string encoded1 = "00000";
            const string encoded2 = "%nSc1";

            // act
            var result1 = Sut.Decode(encoded1);
            var result2 = Sut.Decode(encoded2);

            // assert
            Assert.Equal(result1, result2);
        }
    }
}
