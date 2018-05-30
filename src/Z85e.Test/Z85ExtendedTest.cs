using System;
using System.Security.Cryptography;
using Xunit;

namespace CoenM.Encoding.Test
{
    public class Z85ExtendedTest
    {
        private readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
        private const string HelloWorldString = "HelloWorld";


        [Fact]
        public void HelloWorldDecodeTest()
        {
            Assert.Equal(Z85Extended.Decode(HelloWorldString).ToArray(), _helloWorldBytes);
        }

        [Fact]
        public void HelloWorldEncodeTest()
        {
            Assert.Equal(Z85Extended.Encode(_helloWorldBytes), HelloWorldString);
        }

        [Fact]
        public void SomeEncodingAndDecodingOfPartialsTest()
        {
            byte[] bytes1 = { 0xB5 };
            byte[] bytes2 = { 0xB5, 0x59 };
            byte[] bytes3 = { 0xB5, 0x59, 0xF7 };

            Assert.Equal("2b", Z85Extended.Encode(bytes1));
            Assert.Equal("6Af", Z85Extended.Encode(bytes2));
            Assert.Equal("jt#7", Z85Extended.Encode(bytes3));

            Assert.Equal(bytes1, Z85Extended.Decode("2b").ToArray());
            Assert.Equal(bytes2, Z85Extended.Decode("6Af").ToArray());
            Assert.Equal(bytes3, Z85Extended.Decode("jt#7").ToArray());
        }


        [Fact]
        public void SomeEncodingAndDecodingOfPartialsPrefixedWithFourBytesTest()
        {
            byte[] bytes1 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5 };
            byte[] bytes2 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59 };
            byte[] bytes3 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7 };

            Assert.Equal("Hello2b", Z85Extended.Encode(bytes1));
            Assert.Equal("Hello6Af", Z85Extended.Encode(bytes2));
            Assert.Equal("Hellojt#7", Z85Extended.Encode(bytes3));

            Assert.Equal(bytes1, Z85Extended.Decode("Hello2b").ToArray());
            Assert.Equal(bytes2, Z85Extended.Decode("Hello6Af").ToArray());
            Assert.Equal(bytes3, Z85Extended.Decode("Hellojt#7").ToArray());
        }

        [Fact]
        public void DecodeNullThrowsExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => Z85Extended.Decode(null));
        }

        [Fact]
        public void EncodeNullReturnsEmptyStringTest()
        {
            Assert.Equal(string.Empty, Z85Extended.Encode(null));
        }

        [Fact]
        public void StringToDecodeCannotHaveSizeOneTest()
        {
            // 5n+1 chars is not allowed.
            Assert.Throws<ArgumentException>(() => Z85Extended.Decode("123456"));
        }

        [Fact]
        [Trait(XUnitConst.Catagory, XUnitConst.Categories.StressTest)]
        public void StressTest()
        {
            // arrange
            var bytes = CreatePseudoRandomByteArray(1024 * 1024 * 300, 2343429);

            // act
            var result = Z85.Encode(bytes);

            // assert
            Assert.Equal("Q*}EDu2563cEvhD{]baI.r&ub^P[heR9UY=fIwkM", CreateSha256Z85Encoded(result));
        }

        private static string CreateSha256Z85Encoded(string input)
        {
            var bytes = System.Text.Encoding.Unicode.GetBytes(input);
            var hashstring = new SHA256Managed();
            return Z85.Encode(hashstring.ComputeHash(bytes));
        }

        private static byte[] CreatePseudoRandomByteArray(uint size, int seed)
        {
            var random = new Random(seed);
            var result = new byte[size];
            for (var i = 0; i < size; i++)
                result[i] = (byte) random.Next(0, 255);
            return result;
        }
    }
}