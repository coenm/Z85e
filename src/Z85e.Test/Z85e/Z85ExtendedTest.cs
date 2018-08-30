namespace CoenM.Encoding.Test.Z85e
{
    using Xunit.Categories;

    using System;
    using System.Security.Cryptography;

    using CoenM.Encoding.Test.TestData;

    using Xunit;

    using Sut = Encoding.Z85Extended;

    public class Z85ExtendedTest
    {
        [Fact]
        public void HelloWorldDecodeTest()
        {
            Assert.Equal(Sut.Decode(StrictZ85Samples.HelloWorldEncoded), StrictZ85Samples.HelloWorldData);
        }

        [Fact]
        public void HelloWorldEncodeTest()
        {
            Assert.Equal(Sut.Encode(StrictZ85Samples.HelloWorldData), StrictZ85Samples.HelloWorldEncoded);
        }

        [Fact]
        public void SomeEncodingAndDecodingOfPartialsTest()
        {
            byte[] bytes1 = { 0xB5 };
            byte[] bytes2 = { 0xB5, 0x59 };
            byte[] bytes3 = { 0xB5, 0x59, 0xF7 };

            Assert.Equal("2b", Sut.Encode(bytes1));
            Assert.Equal("6Af", Sut.Encode(bytes2));
            Assert.Equal("jt#7", Sut.Encode(bytes3));

            Assert.Equal(bytes1, Sut.Decode("2b"));
            Assert.Equal(bytes2, Sut.Decode("6Af"));
            Assert.Equal(bytes3, Sut.Decode("jt#7"));
        }


        [Fact]
        public void SomeEncodingAndDecodingOfPartialsPrefixedWithFourBytesTest()
        {
            byte[] bytes1 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5 };
            byte[] bytes2 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59 };
            byte[] bytes3 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7 };

            Assert.Equal("Hello2b", Sut.Encode(bytes1));
            Assert.Equal("Hello6Af", Sut.Encode(bytes2));
            Assert.Equal("Hellojt#7", Sut.Encode(bytes3));

            Assert.Equal(bytes1, Sut.Decode("Hello2b"));
            Assert.Equal(bytes2, Sut.Decode("Hello6Af"));
            Assert.Equal(bytes3, Sut.Decode("Hellojt#7"));
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
        public void StringToDecodeCannotHaveSizeOneTest()
        {
            // 5n+1 chars is not allowed.
            Assert.Throws<ArgumentException>(() => Z85Extended.Decode("123456"));
        }

        [Fact]
        [SystemTest]
        public void StressTest()
        {
            // arrange
            var bytes = CreatePseudoRandomByteArray(1024 * 1024 * 300, 2343429);

            // act
            var result = Sut.Encode(bytes);

            // assert
            Assert.Equal("Q*}EDu2563cEvhD{]baI.r&ub^P[heR9UY=fIwkM", CreateSha256Z85Encoded(result));
        }

        private static string CreateSha256Z85Encoded(string input)
        {
            var bytes = System.Text.Encoding.Unicode.GetBytes(input);
            var hashstring = new SHA256Managed();
            return Sut.Encode(hashstring.ComputeHash(bytes));
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