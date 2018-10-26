namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    using System.Collections.Generic;

    using Xunit;

    internal class Z85Base64EncodeScenarios : TheoryData<Z85EncodeScenario, Base64EncodeScenario>
    {
        public Z85Base64EncodeScenarios()
        {
            var encodedCharsCollection = new List<BytesToEncode>
            {
                // zero chars
                new BytesToEncode(new byte[0], new byte[0]),

                // exactly one block
                new BytesToEncode(new byte[] { 0x86, 0x4F, 0xD2, 0x6F }, new byte[] { 0x86, 0x4F, 0xD2 }),
            };

            foreach (var encodedChars in encodedCharsCollection)
            {
                foreach (var isFinalBlock in new[] { true, false })
                {
                    Add(
                        new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, 0),
                        new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, 0));

                    Add(
                        new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, Z85EncodeScenario.BlockSize - 1),
                        new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, Base64EncodeScenario.BlockSize - 1));

                    Add(
                        new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, Z85EncodeScenario.BlockSize), // 5 chars decode to 4 bytes (exactly one block)
                        new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, Base64EncodeScenario.BlockSize)); // 4 chars decode to 3 bytes (exactly one block)

                    Add(
                        new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, Z85EncodeScenario.BlockSize * 2),
                        new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, Base64EncodeScenario.BlockSize * 2));
                }
            }

            /*
              // less then one block chars
              foreach (var encodedChars in new[] { new BytesToEncode(new byte[] { 0x86, 0x4F, 0xD2 }, new byte[] { 0x86, 0x4F }) })
              foreach (var isFinalBlock in new[] { true, false })
              {
                  Add(
                      new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, 0),
                      new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, 0));

            //      Add(
            //          new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, Z85EncodeScenario.BLOCK_SIZE - 1),
            //          new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, Base64EncodeScenario.BLOCK_SIZE - 1));
            //
            //      Add(
            //          new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, Z85EncodeScenario.BLOCK_SIZE), // 5 chars decode to 4 bytes (exactly one block)
            //          new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, Base64EncodeScenario.BLOCK_SIZE)); // 4 chars decode to 3 bytes (exactly one block)

                  Add(
                      new Z85EncodeScenario(encodedChars.Z85, isFinalBlock, Z85EncodeScenario.BLOCK_SIZE * 2),
                      new Base64EncodeScenario(encodedChars.Base64, isFinalBlock, Base64EncodeScenario.BLOCK_SIZE * 2));
              }
            */
        }

        private struct BytesToEncode
        {
            public BytesToEncode(byte[] z85, byte[] base64)
            {
                Z85 = z85;
                Base64 = base64;
            }

            public byte[] Z85 { get; }

            public byte[] Base64 { get; }
        }
    }
}
