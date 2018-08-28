using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    internal class NeedMoreDataDecodeScenario : TheoryData<DecodeData>
    {
        public NeedMoreDataDecodeScenario()
        {
            // Add(new DecodeData(GetHelloString(0), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(1), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(2), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(3), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(4), new byte[0], 0, 0));
            // Add(new DecodeData(GetHelloString(5), HelloBytes, 5, 4));
            Add(new DecodeData(GetHelloString(6), HelloBytes, 5, 4));
            Add(new DecodeData(GetHelloString(7), HelloBytes, 5, 4));
            Add(new DecodeData(GetHelloString(8), HelloBytes, 5, 4));
        }

        private static string GetHelloString(int charCount) => "HelloWorld".Substring(0, charCount);

        private static byte[] HelloBytes => new byte[] { 0x86, 0x4F, 0xD2, 0x6F };
    }
}