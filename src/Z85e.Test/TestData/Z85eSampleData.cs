namespace CoenM.Encoding.Test.TestData
{
    using System;
    using System.Linq;

    internal static class Z85eSampleData
    {
        public static string GetHelloString(int charCount)
        {
            return charCount switch
            {
                0 => string.Empty,
                1 => "H", // should not be possible
                2 => "1N",
                3 => "4:H",
                4 => "esp$",
                5 => "Hello",
                6 => "HelloW", // should not be possible
                7 => "Hello2b",
                8 => "Hello6Af",
                9 => "Hellojt#7",
                10 => "HelloWorld",
                _ => throw new NotImplementedException()
            };
        }

        public static byte[] HelloWorldBytes(int count)
        {
            return new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B }.Take(count).ToArray();
        }
    }
}
