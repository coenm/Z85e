using Xunit;

namespace CoenM.Z85e.Test
{
    public class Z85ExtendedTest
    {
        private readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
        private const string HelloWorldString = "HelloWorld";

        
        [Fact]
        public void HelloWorldDecodeTest()
        {
            Assert.Equal(Z85Extended.Decode(HelloWorldString), _helloWorldBytes);
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

            Assert.Equal(Z85Extended.Encode(bytes1), "2b");
            Assert.Equal(Z85Extended.Encode(bytes2), "6Af");
            Assert.Equal(Z85Extended.Encode(bytes3), "jt#7");

            Assert.Equal(Z85Extended.Decode("2b"), bytes1);
            Assert.Equal(Z85Extended.Decode("6Af"), bytes2);
            Assert.Equal(Z85Extended.Decode("jt#7"), bytes3);
        }


        [Fact]
        public void SomeEncodingAndDecodingOfPartialsPrefixedWithFourBytesTest()
        {
            byte[] bytes1 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5 };
            byte[] bytes2 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59 };
            byte[] bytes3 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7 };

            Assert.Equal(Z85Extended.Encode(bytes1), "Hello2b");
            Assert.Equal(Z85Extended.Encode(bytes2), "Hello6Af");
            Assert.Equal(Z85Extended.Encode(bytes3), "Hellojt#7");

            Assert.Equal(Z85Extended.Decode("Hello2b"), bytes1);
            Assert.Equal(Z85Extended.Decode("Hello6Af"), bytes2);
            Assert.Equal(Z85Extended.Decode("Hellojt#7"), bytes3);
        }
    }
}