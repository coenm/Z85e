using System.Buffers;
using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    internal class AllEncodeScenarios : TheoryData<EncodeInputData, EncodeExpectedData>
    {
        public AllEncodeScenarios()
        {
            //
            // Encode 0 bytes.
            //
            int bytesToEncode = 0;
            var inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var doneResult = new EncodeExpectedData(OperationStatus.Done, 0, 0, string.Empty);

            Add(Input(inputBytes, false), doneResult);
            Add(Input(inputBytes, true), doneResult);

            //
            // Encode 1 byte.
            //
            bytesToEncode = 1;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 0, 0, string.Empty);
            var paddedTwoCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedTwoCharsFinalResult); // different

            //
            // Encode 2 bytes.
            //
            bytesToEncode = 2;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedThreeCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedThreeCharsFinalResult); // different

            //
            // Encode 3 bytes.
            //
            bytesToEncode = 3;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedFourCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedFourCharsFinalResult); // different

            //
            // Encode 4 bytes.
            // For all the same.
            //
            bytesToEncode = 4;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var helloDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));
            var helloDecodedNeedMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false), helloDecodedNeedMoreDataResult);  //todo fix, test is probably ok
            Add(Input(inputBytes, true), helloDecodedDoneResult);

            //
            // Encode 5 bytes.
            // first four bytes will be encoded, 5th byte depends on mode.
            //
            bytesToEncode = 5;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 4, 5, Z85eSampleData.GetHelloString(5));
            var paddedSevenCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 5, 7, Z85eSampleData.GetHelloString(7));

            Add(Input(inputBytes, false), needMoreDataResult); //todo fix, test is probably ok
            Add(Input(inputBytes, true), paddedSevenCharsFinalResult); // different

            //
            // Encode 6 bytes.
            //
            bytesToEncode = 6;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedEightCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 6, 8, Z85eSampleData.GetHelloString(8));

            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedEightCharsFinalResult);

            //
            // Encode 7 bytes.
            //
            bytesToEncode = 7;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedNineCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 7, 9, Z85eSampleData.GetHelloString(9));

            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedNineCharsFinalResult);  // different

            //
            // Encode 8 bytes.
            //
            bytesToEncode = 8;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var helloWorldDecodedNeedMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, bytesToEncode, bytesToEncode + 2, Z85eSampleData.GetHelloString(bytesToEncode + 2));
            var helloWorldDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 2, Z85eSampleData.GetHelloString(bytesToEncode + 2));

            Add(Input(inputBytes, false), helloWorldDecodedNeedMoreDataResult);
            Add(Input(inputBytes, true), helloWorldDecodedDoneResult);
        }

        private static EncodeInputData Input(byte[] input, bool finalBlock) => new EncodeInputData(input, finalBlock);
    }
}