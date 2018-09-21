namespace CoenM.Encoding.Test.TestInternals
{
    using System;
    using System.Security.Cryptography;

    internal static class MyData
    {
        public static string CreateSha256Base64Encoded(string input)
        {
            var bytes = System.Text.Encoding.Unicode.GetBytes(input);
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(bytes));
        }

        public static byte[] CreatePseudoRandomByteArray(uint size, int seed)
        {
            var random = new Random(seed);
            var result = new byte[size];
            for (var i = 0; i < size; i++)
                result[i] = (byte)random.Next(0, 255);
            return result;
        }
    }
}