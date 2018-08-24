namespace CoenM.Encoding.Test.TestData
{
    using Xunit;

    internal class Z85Samples : TheoryData<byte[], string>
    {
        private static readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
        private const string HelloWorldString = "HelloWorld";

        public Z85Samples()
        {
            Add(
                _helloWorldBytes,
                HelloWorldString);

            Add(
                new byte[]
                    {
                        0xB5, 0x59, 0xF7, 0x5B,
                        0xB5, 0x59, 0xF7, 0x5B,
                        0x86, 0x4F, 0xD2, 0x6F,
                    },
                "WorldWorldHello");
        }


        public static string HelloWorldEncoded { get; } = HelloWorldString;

        public static byte[] HelloWorldData { get; } = _helloWorldBytes;
    }
}