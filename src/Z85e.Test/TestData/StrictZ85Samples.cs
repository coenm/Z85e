namespace CoenM.Encoding.Test.TestData
{
    using Xunit;

    internal class StrictZ85Samples : TheoryData<byte[], string>
    {
        private const string HelloWorldString = "HelloWorld";
        private static readonly byte[] HelloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };

        public StrictZ85Samples()
        {
            Add(
                HelloWorldBytes,
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

        public static byte[] HelloWorldData { get; } = HelloWorldBytes;
    }
}
