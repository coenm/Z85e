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
            var needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 0, 0, string.Empty);
            var destinationTooSmallZeroBytesResult = new EncodeExpectedData(OperationStatus.DestinationTooSmall, 0, 0, string.Empty);
            var destinationTooSmallFourBytesResult = new EncodeExpectedData(OperationStatus.DestinationTooSmall, 4, 5, Z85eSampleData.GetHelloString(5));
            var doneResult = new EncodeExpectedData(OperationStatus.Done, 0, 0, string.Empty);

            Add(Input(inputBytes, false, 0), needMoreDataResult);
            Add(Input(inputBytes, true, 0), doneResult);
            Add(Input(inputBytes, false, 10), needMoreDataResult);
            Add(Input(inputBytes, true, 10), doneResult);

            //
            // Encode 1 byte.
            //
            bytesToEncode = 1;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedTwoCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false, 0), needMoreDataResult);
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false, 2), needMoreDataResult);
            Add(Input(inputBytes, true, 2), paddedTwoCharsFinalResult); // different

            //
            // Encode 2 bytes.
            //
            bytesToEncode = 2;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedThreeCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false, 0), needMoreDataResult);
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedThreeCharsFinalResult); // different

            //
            // Encode 3 bytes.
            //
            bytesToEncode = 3;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedFourCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 1, Z85eSampleData.GetHelloString(bytesToEncode + 1));

            Add(Input(inputBytes, false, 0), needMoreDataResult);
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
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

            Add(Input(inputBytes, false, 0), destinationTooSmallZeroBytesResult); // different
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false), helloDecodedNeedMoreDataResult);
            Add(Input(inputBytes, true), helloDecodedDoneResult);

            //
            // Encode 5 bytes.
            // first four bytes will be encoded, 5th byte depends on mode.
            //
            bytesToEncode = 5;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            needMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, 4, 5, Z85eSampleData.GetHelloString(5));
            var paddedSevenCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 5, 7, Z85eSampleData.GetHelloString(7));

            Add(Input(inputBytes, false, 0), destinationTooSmallZeroBytesResult);  // different
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false, 5), needMoreDataResult);  // different
            Add(Input(inputBytes, true, 5), destinationTooSmallFourBytesResult);
            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedSevenCharsFinalResult); // different

            //
            // Encode 6 bytes.
            //
            bytesToEncode = 6;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedEightCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 6, 8, Z85eSampleData.GetHelloString(8));

            Add(Input(inputBytes, false, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false, 5), needMoreDataResult);  // different
            Add(Input(inputBytes, true, 5), destinationTooSmallFourBytesResult);
            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedEightCharsFinalResult);

            //
            // Encode 7 bytes.
            //
            bytesToEncode = 7;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var paddedNineCharsFinalResult = new EncodeExpectedData(OperationStatus.Done, 7, 9, Z85eSampleData.GetHelloString(9));

            Add(Input(inputBytes, false, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false, 5), needMoreDataResult);  // different
            Add(Input(inputBytes, true, 5), destinationTooSmallFourBytesResult);
            Add(Input(inputBytes, false), needMoreDataResult);
            Add(Input(inputBytes, true), paddedNineCharsFinalResult);  // different

            //
            // Encode 8 bytes.
            //
            bytesToEncode = 8;
            inputBytes = Z85eSampleData.HelloWorldBytes(bytesToEncode);
            var helloWorldDecodedNeedMoreDataResult = new EncodeExpectedData(OperationStatus.NeedMoreData, bytesToEncode, bytesToEncode + 2, Z85eSampleData.GetHelloString(bytesToEncode + 2));
            var helloWorldDecodedDoneResult = new EncodeExpectedData(OperationStatus.Done, bytesToEncode, bytesToEncode + 2, Z85eSampleData.GetHelloString(bytesToEncode + 2));

            Add(Input(inputBytes, false, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, true, 0), destinationTooSmallZeroBytesResult);
            Add(Input(inputBytes, false, 5), destinationTooSmallFourBytesResult);  // different
            Add(Input(inputBytes, true, 5), destinationTooSmallFourBytesResult);
            Add(Input(inputBytes, false), helloWorldDecodedNeedMoreDataResult);
            Add(Input(inputBytes, true), helloWorldDecodedDoneResult);
        }

        private static EncodeInputData Input(byte[] input, bool finalBlock, int destinationLength = -1) => new EncodeInputData(input, finalBlock, destinationLength);
    }
}