using System.Buffers;
using System.Linq;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    internal class AllDecodeScenarios : TheoryData<DecodeInputData, DecodeExpectedData>
    {
        public AllDecodeScenarios()
        {
            // Decode 0 character.
            var doneResult = new DecodeExpectedData(OperationStatus.Done, 0, 0, new byte[0]);
            var encodedString = string.Empty;

            Add(Input(encodedString, Z85Mode.Padding, false), doneResult);
            Add(Input(encodedString, Z85Mode.Padding, true), doneResult);
            Add(Input(encodedString, Z85Mode.Strict, false), doneResult);
            Add(Input(encodedString, Z85Mode.Strict, true), doneResult);


            // Decode 1 character.
            // One character is special and can never be decoded.
            var needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 0, 0, new byte[0]);
            var invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 0, 0, new byte[0]);
            encodedString = GetHelloString(1);

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), invalidDataResult);
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 2 chars.
            //
            encodedString = GetHelloString(2);
            var paddedOneByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 2, 1, HelloBytes(1));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedOneByteFinalResult); // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 3 chars.
            //
            encodedString = GetHelloString(3);
            var paddedTwoByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 3, 2, HelloBytes(2));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedTwoByteFinalResult); // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 4 chars.
            //
            encodedString = GetHelloString(4);
            var paddedThreeByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 4, 3, HelloBytes(3));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedThreeByteFinalResult); // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 5 chars.
            // This is allowed in strict Z85
            // In all cases the result should be the same.
            //
            encodedString = GetHelloString(5);
            var helloDecodedDoneResult = new DecodeExpectedData(OperationStatus.Done, 5, 4, HelloBytes(4));

            Add(Input(encodedString, Z85Mode.Padding, false), helloDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Padding, true), helloDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, false), helloDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, true), helloDecodedDoneResult);


            //
            // Decode 6 chars.
            //
            encodedString = GetHelloString(6);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, HelloBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, HelloBytes(4));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), invalidDataResult);
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);


            //
            // Decode 7 chars.
            //
            encodedString = GetHelloString(7);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, HelloBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, HelloBytes(4));
            var paddedFiveByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 7, 5, HelloBytes(5));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedFiveByteFinalResult);  // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 8 chars.
            //
            encodedString = GetHelloString(8);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, HelloBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, HelloBytes(4));
            var paddedSixByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 8, 6, HelloBytes(6));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedSixByteFinalResult);  // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);


            //
            // Decode 9 chars.
            //
            encodedString = GetHelloString(9);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, HelloBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, HelloBytes(4));
            var paddedSevenByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 9, 7, HelloBytes(7));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedSevenByteFinalResult);  // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);


            //
            // Decode 10 chars.
            // This is allowed in strict Z85
            // In all cases the result should be the same.
            //
            encodedString = GetHelloString(10);
            var helloWorldDecodedDoneResult = new DecodeExpectedData(OperationStatus.Done, 10, 8, HelloBytes(8));

            Add(Input(encodedString, Z85Mode.Padding, false), helloWorldDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Padding, true), helloWorldDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, false), helloWorldDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, true), helloWorldDecodedDoneResult);

        }

        private static string GetHelloString(int charCount) => "HelloWorld".Substring(0, charCount);

        private byte[] HelloBytes(int count)
        {
            return new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B } .Take(count).ToArray();
        }

        private DecodeInputData Input(string input, Z85Mode mode, bool finalBlock) => new DecodeInputData(input, mode, finalBlock);
    }
}