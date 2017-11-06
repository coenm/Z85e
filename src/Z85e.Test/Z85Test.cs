using System;
using Xunit;

namespace CoenM.Z85e.Test
{
    public class Z85Test
    {
        private readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
        private const string HelloWorldString = "HelloWorld";

        [Fact]
        public void HelloWorldDecodeTest()
        {
            Assert.Equal(Z85.Decode(HelloWorldString), _helloWorldBytes);
        }

        [Fact]
        public void HelloWorldEncodeTest()
        {
            Assert.Equal(Z85.Encode(_helloWorldBytes), HelloWorldString);
        }

        [Fact]
        public void EncodeThrowsExceptionWhenInputIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => Z85.Encode(null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void EncodeThrowsExceptionWhenInputHasWrongSizeTest(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Z85.Encode(new byte[size]));
        }

        [Fact]
        public void DecodeThrowsExceptionWhenInputIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => Z85.Decode(null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Z85.Decode(new String('a', size)));
        }


        [Fact]
        public void MultipleEncodedStringsDecodeToSameBytes()
        {
            // arrange
            var encoded1 = "00000";
            var encoded2 = "%nSc1";

            // act
            var result1 = Z85.Decode(encoded1);
            var result2 = Z85.Decode(encoded2);

            // assert
            Assert.Equal(result1, result2);
        }
    }
}