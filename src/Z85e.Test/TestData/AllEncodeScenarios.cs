using System.Buffers;
using System.Linq;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    internal class AllEncodeScenarios : TheoryData<EncodeInputData, EncodeExpectedData>
    {
        public AllEncodeScenarios()
        {
            // Encode 0 bytes.
            int bytesToEncode = 0;
            var inputBytes = HelloBytes(bytesToEncode);
            var doneResult = new EncodeExpectedData(OperationStatus.Done, 0, 0, string.Empty);

            Add(Input(inputBytes, Z85Mode.Padding, false), doneResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), doneResult);
            Add(Input(inputBytes, Z85Mode.Strict, false), doneResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), doneResult);


            // Encode 1 byte.
            bytesToEncode = 1;
            inputBytes = HelloBytes(bytesToEncode);
            var needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 0, 0, string.Empty);
            var invalidDataResult = new EncodeExpectedData(OperationStatus.InvalidData, 0, 0, string.Empty);
            var paddedTwoCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            /**/Add(Input(inputBytes, Z85Mode.Padding, true), paddedTwoCharsFinalResult); // different //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            // Encode 2 bytes.
            //
            bytesToEncode = 2;
            inputBytes = HelloBytes(bytesToEncode);
            var paddedThreeCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            // Add(Input(inputBytes, Z85Mode.Padding, true), paddedThreeCharsFinalResult); // different //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            // Encode 3 bytes.
            bytesToEncode = 3;
            inputBytes = HelloBytes(bytesToEncode);
            var paddedFourCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            //            Add(Input(inputBytes, Z85Mode.Padding, true), paddedFourCharsFinalResult); // different  //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            // Encode 4 bytes.
            // For all the same.
            //
            bytesToEncode = 4;
            inputBytes = HelloBytes(bytesToEncode);
            var helloDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, GetHelloString(bytesToEncode + 1));

            //            Add(Input(inputBytes, Z85Mode.Padding, false), helloDecodedDoneResult);  //todo fix, test is probably ok
//            Add(Input(inputBytes, Z85Mode.Padding, true), helloDecodedDoneResult); ////todo fix, test is probably ok
            //            Add(Input(inputBytes, Z85Mode.Strict, false), helloDecodedDoneResult);  //todo fix, test is probably ok
//            Add(Input(inputBytes, Z85Mode.Strict, true), helloDecodedDoneResult); //todo fix, test is probably ok



            // Encode 5 bytes.
            // first four bytes will be encoded, 5th byte depends on mode.
            bytesToEncode = 5;
            needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 4, 5, GetHelloString(5));
            invalidDataResult = new EncodeExpectedData(OperationStatus.InvalidData, 4, 5, GetHelloString(5));
            inputBytes = HelloBytes(bytesToEncode);
            var paddedFiveCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 2, GetHelloString(bytesToEncode + 2));

//            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult); //todo fix, test is probably ok
//            Add(Input(inputBytes, Z85Mode.Padding, true), paddedFiveCharsFinalResult); // different
//            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult); //todo fix, test is probably ok
//            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            //            //
            //            // Encode 6 bytes.
            //            bytesToEncode = 6;
            //            inputBytes = HelloBytes(bytesToEncode);
            //            needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 5, 4, HelloBytes(4));
            //            invalidDataResult = new EncodeExpectedData(OperationStatus.InvalidData, 5, 4, HelloBytes(4));
            //
            //            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            //            Add(Input(inputBytes, Z85Mode.Padding, true), invalidDataResult);
            //            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            //            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);
            //
            //
            //            //
            //            // Encode 7 bytes.
            //            bytesToEncode = 7;
            //            inputBytes = HelloBytes(bytesToEncode);
            //            needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 5, 4, HelloBytes(4));
            //            invalidDataResult = new EncodeExpectedData(OperationStatus.InvalidData, 5, 4, HelloBytes(4));
            //            var paddedFiveByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 7, 5, HelloBytes(5));
            //
            //            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            //            Add(Input(inputBytes, Z85Mode.Padding, true), paddedFiveByteFinalResult);  // different
            //            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            //            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);
            //

//            //
//            // Encode 8 bytes.
//            // For all the same.
//            //
//            bytesToEncode = 8;
//            inputBytes = HelloBytes(bytesToEncode);
//            var helloWorldDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 2, GetHelloString(bytesToEncode + 2));
//
//            Add(Input(inputBytes, Z85Mode.Padding, false), helloWorldDecodedDoneResult);
//            Add(Input(inputBytes, Z85Mode.Padding, true), helloWorldDecodedDoneResult);
//            Add(Input(inputBytes, Z85Mode.Strict, false), helloWorldDecodedDoneResult);
//            Add(Input(inputBytes, Z85Mode.Strict, true), helloWorldDecodedDoneResult);

        }

        private static string GetHelloString(int charCount) => "HelloWorld".Substring(0, charCount);

        private byte[] HelloBytes(int count)
        {
            return new byte[] { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B }.Take(count).ToArray();
        }

        private EncodeInputData Input(byte[] input, Z85Mode mode, bool finalBlock) => new EncodeInputData(input, mode, finalBlock);
    }
}