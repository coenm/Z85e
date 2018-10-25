namespace CoenM.Encoding.Test.TestData
{
    using System;
    using System.Linq;

    internal static class Z85eSampleData
    {
        public static string GetHelloString(int charCount)
        {
            switch (charCount)
            {
                case 0: return string.Empty;
                case 1: return "H"; // should not be possible
                case 2: return "1N";
                case 3: return "4:H";
                case 4: return "esp$";
                case 5: return "Hello";
                case 6: return "HelloW"; // should not be possible
                case 7: return "Hello2b";
                case 8: return "Hello6Af";
                case 9: return "Hellojt#7";
                case 10: return "HelloWorld";

                default:
                    throw new NotImplementedException();
            }
        }

        public static byte[] HelloWorldBytes(int count)
        {
            return new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B }.Take(count).ToArray();
        }
    }
}
