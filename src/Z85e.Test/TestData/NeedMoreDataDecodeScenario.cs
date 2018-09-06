using System;
using System.Buffers;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{

    internal class ModeOptions : TheoryData<Z85Mode>
    {
        public ModeOptions()
        {
            Add(Z85Mode.Padding);
            Add(Z85Mode.Strict);
        }
    }

    internal class ModeAndFinalBlockOptions : TheoryData<Z85Mode, bool>
    {
        public ModeAndFinalBlockOptions()
        {
            Add(Z85Mode.Padding, true);
            Add(Z85Mode.Padding, false);
            Add(Z85Mode.Strict, true);
            Add(Z85Mode.Strict, false);
        }
    }


    internal class NeedMoreDataEncodeScenario : TheoryData<EncodeData>
    {
        public NeedMoreDataEncodeScenario()
        {
            // Add(new EncodeData(GetHelloData(1), GetHelloString(0), 0, 0, OperationStatus.NeedMoreData));
            // Add(new EncodeData(GetHelloData(2), GetHelloString(0), 0, 0, OperationStatus.NeedMoreData));
            // Add(new EncodeData(GetHelloData(3), GetHelloString(0), 0, 0, OperationStatus.NeedMoreData));

            Add(new EncodeData(GetHelloData(5), GetHelloString(5), 4, 5, OperationStatus.NeedMoreData));
            // Add(new EncodeData(GetHelloData(6), GetHelloString(5), 4, 5, OperationStatus.NeedMoreData));
            // Add(new EncodeData(GetHelloData(7), GetHelloString(5), 4, 5, OperationStatus.NeedMoreData));
        }

        private static byte[] GetHelloData(int count) => HelloBytes.AsSpan().Slice(0, count).ToArray();

        private static string GetHelloString(int charCount) => "WorldWorldHello".Substring(0, charCount);

        private static byte[] HelloBytes => new byte[]
            {
                0xB5, 0x59, 0xF7, 0x5B,
                0xB5, 0x59, 0xF7, 0x5B,
                0x86, 0x4F, 0xD2, 0x6F,
            };
    }


    internal class NeedMoreDataDecodeScenario : TheoryData<DecodeData>
    {
        public NeedMoreDataDecodeScenario()
        {
            Add(new DecodeData(GetHelloString(1), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(2), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(3), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(4), new byte[0], 0, 0));

            Add(new DecodeData(GetHelloString(6), HelloBytes, 5, 4));
            Add(new DecodeData(GetHelloString(7), HelloBytes, 5, 4));
            Add(new DecodeData(GetHelloString(8), HelloBytes, 5, 4));
        }

        private static string GetHelloString(int charCount) => "HelloWorld".Substring(0, charCount);

        private static byte[] HelloBytes => new byte[] { 0x86, 0x4F, 0xD2, 0x6F };
    }

    internal class NeedMoreDataDecodeScenarioC : TheoryData<DecodeData>
    {
        public NeedMoreDataDecodeScenarioC()
        {
            Add(new DecodeData(GetHelloString(1), new byte[0], 0, 0));
            Add(new DecodeData(GetHelloString(6), HelloBytes, 5, 4));
        }

        private static string GetHelloString(int charCount) => "HelloWorld".Substring(0, charCount);

        private static byte[] HelloBytes => new byte[] { 0x86, 0x4F, 0xD2, 0x6F };
    }


    internal class PaddedZ85Scenario : TheoryData<DecodeData>
    {
        public PaddedZ85Scenario()
        {
            Add(new DecodeData(GetHelloString(2), new byte[1], 0, 0));
            Add(new DecodeData(GetHelloString(3), new byte[2], 0, 0));
            Add(new DecodeData(GetHelloString(4), new byte[3], 0, 0));

            Add(new DecodeData(GetHelloString(7), HelloBytes, 5, 4));
            Add(new DecodeData(GetHelloString(8), HelloBytes, 5, 4));
        }

        private static string GetHelloString(int charCount) => "HelloWorld".Substring(0, charCount);

        private static byte[] HelloBytes => new byte[] { 0x86, 0x4F, 0xD2, 0x6F };
    }
}