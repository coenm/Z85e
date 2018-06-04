using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Xunit;

namespace CoenM.Encoding.Test
{
    public class Z85ExtendedTest
    {
        [Theory]
        [MemberData(nameof(Z85Maps))]
        public void SomeEncodingAndDecodingOfPartialsTest(byte[] rawData, string z85EncodedData)
        {
            Assert.Equal(z85EncodedData, Z85Extended.Encode(rawData));
            Assert.Equal(rawData, Z85Extended.Decode(z85EncodedData).ToArray());
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
            var result = Z85.EncodeSpan(bytes);

            // assert
            Assert.Equal("Q*}EDu2563cEvhD{]baI.r&ub^P[heR9UY=fIwkM", CreateSha256Z85Encoded(result));
        }


        public static IEnumerable<object[]> Z85Maps
        {
            get
            {
                yield return new object[] { new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B }, "HelloWorld" };
                yield return new object[] { new byte[] { 0xB5 }, "2b"};
                yield return new object[] { new byte[] { 0xB5, 0x59 }, "6Af"};
                yield return new object[] { new byte[] { 0xB5, 0x59, 0xF7 }, "jt#7"};
                yield return new object[] { new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5 }, "Hello2b"};
                yield return new object[] { new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59 }, "Hello6Af"};
                yield return new object[] { new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7 }, "Hellojt#7"};
            }
        }


        private static string CreateSha256Z85Encoded(string input)
        {
            var bytes = System.Text.Encoding.Unicode.GetBytes(input);
            var hashstring = new SHA256Managed();
            return Z85.EncodeSpan(hashstring.ComputeHash(bytes));
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