namespace CoenM.Encoding.Test.TestData
{
    using Xunit;

    internal class StrictZ85Samples : TheoryData<byte[], string>
    {
        private const string HELLO_WORLD_STRING = "HelloWorld";
        private static readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };

        public StrictZ85Samples()
        {
            Add(
                _helloWorldBytes,
                HELLO_WORLD_STRING);

            Add(
                new byte[]
                    {
                        0xB5, 0x59, 0xF7, 0x5B,
                        0xB5, 0x59, 0xF7, 0x5B,
                        0x86, 0x4F, 0xD2, 0x6F,
                    },
                "WorldWorldHello");
        }


        public static string HelloWorldEncoded { get; } = HELLO_WORLD_STRING;

        public static byte[] HelloWorldData { get; } = _helloWorldBytes;
    }
}