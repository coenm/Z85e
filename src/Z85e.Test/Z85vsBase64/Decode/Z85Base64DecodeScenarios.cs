using System.Collections.Generic;
using Xunit;

namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    internal class Z85Base64DecodeScenarios : TheoryData<Z85DecodeScenario, Base64DecodeScenario>
    {
        public Z85Base64DecodeScenarios()
        {
            var encodedCharsCollection = new List<EncodedChars>
            {
                // zero chars
                new EncodedChars(string.Empty, string.Empty),

                // invalid (length) chars
                new EncodedChars("a", "a"),

                // exactly one block
                new EncodedChars("aaaaa", "aaaa"),
            };

            foreach (var encodedChars in encodedCharsCollection)
            foreach (var isFinalBlock in new[] {true, false})
            {
                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, 0),
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock, 0));

                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, Z85DecodeScenario.BLOCK_SIZE - 1),
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
                        Base64DecodeScenario.BLOCK_SIZE - 1));

                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock,
                        Z85DecodeScenario.BLOCK_SIZE), // 5 chars decode to 4 bytes (exactly one block)
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
                        Base64DecodeScenario.BLOCK_SIZE)); // 4 chars decode to 3 bytes (exactly one block)

                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, Z85DecodeScenario.BLOCK_SIZE * 2),
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
                        Base64DecodeScenario.BLOCK_SIZE * 2));
            }


            // less then one block chars
            foreach (var encodedChars in new[] { new EncodedChars("aa", "aa") })
            foreach (var isFinalBlock in new[] { false })
            {
                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, 0),
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock, 0));

                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, Z85DecodeScenario.BLOCK_SIZE - 1),
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
                        Base64DecodeScenario.BLOCK_SIZE - 1));

                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock,
                        Z85DecodeScenario.BLOCK_SIZE), // 5 chars decode to 4 bytes (exactly one block)
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
                        Base64DecodeScenario.BLOCK_SIZE)); // 4 chars decode to 3 bytes (exactly one block)

                Add(
                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, Z85DecodeScenario.BLOCK_SIZE * 2),
                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
                        Base64DecodeScenario.BLOCK_SIZE * 2));
            }
//            foreach (var encodedChars in new[] { new EncodedChars("aa", "aa==") })
//            foreach (var isFinalBlock in new[] { true })
//            {
//                Add(
//                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, 0),
//                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock, 0));
//
//                Add(
//                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, Z85DecodeScenario.BLOCK_SIZE - 1),
//                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
//                        Base64DecodeScenario.BLOCK_SIZE - 1));
//
//                Add(
//                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock,
//                        Z85DecodeScenario.BLOCK_SIZE), // 5 chars decode to 4 bytes (exactly one block)
//                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
//                        Base64DecodeScenario.BLOCK_SIZE)); // 4 chars decode to 3 bytes (exactly one block)
//
//                Add(
//                    new Z85DecodeScenario(encodedChars.Z85, isFinalBlock, Z85DecodeScenario.BLOCK_SIZE * 2),
//                    new Base64DecodeScenario(encodedChars.Base64, isFinalBlock,
//                        Base64DecodeScenario.BLOCK_SIZE * 2));
//            }

        }

        private struct EncodedChars
        {
            public EncodedChars(string z85, string base64)
            {
                Z85 = z85;
                Base64 = base64;
            }

            public string Z85 { get; }
            public string Base64 { get; }
        }
    }
}