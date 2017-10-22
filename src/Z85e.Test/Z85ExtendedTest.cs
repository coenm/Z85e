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
        public void HelloWorldDecode1Test()
        {
            Assert.Equal(Z85Extended.Decode(HelloWorldString), _helloWorldBytes);
        }

        [Fact]
        public void HelloWorldEncode1Test()
        {
            byte[] bytes0 = { 0x86, 0x4F, 0xD2, 0x6F };
            byte[] bytes1 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5 };
            byte[] bytes2 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59 };
            byte[] bytes3 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7 };
            byte[] bytes4 = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };

            Assert.Equal(Z85Extended.Encode(bytes0), "Hello");
            Assert.Equal(Z85Extended.Encode(bytes1), "HelloWeZgb3");
            Assert.Equal(Z85Extended.Encode(bytes2), "HelloWoiFf2");
            Assert.Equal(Z85Extended.Encode(bytes3), "HelloWork71");
            Assert.Equal(Z85Extended.Encode(bytes4), "HelloWorld");



            Assert.Equal(Z85Extended.Decode("Hello"), bytes0);
            Assert.Equal(Z85Extended.Decode("HelloWeZgb3"), bytes1);
            Assert.Equal(Z85Extended.Decode("HelloWoiFf2"), bytes2);
            Assert.Equal(Z85Extended.Decode("HelloWork71"), bytes3);
            Assert.Equal(Z85Extended.Decode("HelloWorld"), bytes4);
        }
    }
}