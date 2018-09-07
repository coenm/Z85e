using System.Buffers;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    internal class AllEncodeScenarios : TheoryData<EncodeInputData, EncodeExpectedData>
    {
        public AllEncodeScenarios()
        {
            // Encode 0 bytes.
            int bytesToEncode = 0;
            var inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var doneResult = new EncodeExpectedData(OperationStatus.Done, 0, 0, string.Empty);

            Add(Input(inputBytes, Z85Mode.Padding, false), doneResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), doneResult);
            Add(Input(inputBytes, Z85Mode.Strict, false), doneResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), doneResult);


            // Encode 1 byte.
            bytesToEncode = 1;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 0, 0, string.Empty);
            var invalidDataResult = new EncodeExpectedData(OperationStatus.InvalidData, 0, 0, string.Empty);
            var paddedTwoCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), paddedTwoCharsFinalResult); // different //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            // Encode 2 bytes.
            //
            bytesToEncode = 2;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedThreeCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), paddedThreeCharsFinalResult); // different
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            // Encode 3 bytes.
            bytesToEncode = 3;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedFourCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), paddedFourCharsFinalResult); // different
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            //
            // Encode 4 bytes.
            // For all the same.
            //
            bytesToEncode = 4;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var helloDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, Z85Mode.Padding, false), helloDecodedDoneResult);  //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Padding, true), helloDecodedDoneResult);
            Add(Input(inputBytes, Z85Mode.Strict, false), helloDecodedDoneResult);  //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Strict, true), helloDecodedDoneResult); //todo fix, test is probably ok


            // Encode 5 bytes.
            // first four bytes will be encoded, 5th byte depends on mode.
            bytesToEncode = 5;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 4, 5, Z85eSampleData.GetHelloString(5));
            invalidDataResult = new EncodeExpectedData(OperationStatus.InvalidData, 4, 5, Z85eSampleData.GetHelloString(5));
            var paddedSevenCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 5, 7, Z85eSampleData.GetHelloString(7));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult); //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Padding, true), paddedSevenCharsFinalResult); // different
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult); //todo fix, test is probably ok
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult); //todo fix, test is probably ok

            // Encode 6 bytes.
            bytesToEncode = 6;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedEightCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 6, 8, Z85eSampleData.GetHelloString(8));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), paddedEightCharsFinalResult);
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);

            // Encode 7 bytes.
            bytesToEncode = 7;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedNineCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 7, 9, Z85eSampleData.GetHelloString(9));

            Add(Input(inputBytes, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), paddedNineCharsFinalResult);  // different
            Add(Input(inputBytes, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), invalidDataResult);


            // Encode 8 bytes.
            // For all the same.
            bytesToEncode = 8;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var helloWorldDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 2, Z85eSampleData.GetHelloString(bytesToEncode + 2));

            Add(Input(inputBytes, Z85Mode.Padding, false), helloWorldDecodedDoneResult);
            Add(Input(inputBytes, Z85Mode.Padding, true), helloWorldDecodedDoneResult);
            Add(Input(inputBytes, Z85Mode.Strict, false), helloWorldDecodedDoneResult);
            Add(Input(inputBytes, Z85Mode.Strict, true), helloWorldDecodedDoneResult);
        }

        private static EncodeInputData Input(byte[] input, Z85Mode mode, bool finalBlock) => new EncodeInputData(input, mode, finalBlock);
    }
}