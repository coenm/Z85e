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
    }
}