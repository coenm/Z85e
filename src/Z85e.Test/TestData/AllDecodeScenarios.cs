namespace CoenM.Encoding.Test.TestData
{
    using System.Buffers;

    using Xunit;

    internal class AllDecodeScenarios : TheoryData<DecodeInputData, DecodeExpectedData>
    {
        public AllDecodeScenarios()
        {
            // Decode 0 character.
            var doneResult = new DecodeExpectedData(OperationStatus.Done, 0, 0, new byte[0]);
            var encodedString = string.Empty;

            Add(Input(encodedString, false), doneResult);
            Add(Input(encodedString, true), doneResult);

            // Decode 1 character.
            // One character is special and can never be decoded.
            var needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 0, 0, new byte[0]);
            var invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 0, 0, new byte[0]);
            encodedString = Z85eSampleData.GetHelloString(1);

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), invalidDataResult);

            // Decode 2 chars.
            encodedString = Z85eSampleData.GetHelloString(2);
            var paddedOneByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 2, 1, Z85eSampleData.HelloWorldBytes(1));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), paddedOneByteFinalResult); // different

            // Decode 3 chars.
            encodedString = Z85eSampleData.GetHelloString(3);
            var paddedTwoByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 3, 2, Z85eSampleData.HelloWorldBytes(2));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), paddedTwoByteFinalResult); // different

            // Decode 4 chars.
            encodedString = Z85eSampleData.GetHelloString(4);
            var paddedThreeByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 4, 3, Z85eSampleData.HelloWorldBytes(3));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), paddedThreeByteFinalResult); // different

            // Decode 5 chars.
            encodedString = Z85eSampleData.GetHelloString(5);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var helloDecodedDoneResult = new DecodeExpectedData(OperationStatus.Done, 5, 4, Z85eSampleData.HelloWorldBytes(4));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), helloDecodedDoneResult);

            // Decode 6 chars.
            encodedString = Z85eSampleData.GetHelloString(6);
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, Z85eSampleData.HelloWorldBytes(4));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), invalidDataResult);

            // Decode 7 chars.
            encodedString = Z85eSampleData.GetHelloString(7);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var paddedFiveByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 7, 5, Z85eSampleData.HelloWorldBytes(5));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), paddedFiveByteFinalResult);  // different

            // Decode 8 chars.
            encodedString = Z85eSampleData.GetHelloString(8);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var paddedSixByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 8, 6, Z85eSampleData.HelloWorldBytes(6));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), paddedSixByteFinalResult);  // different

            // Decode 9 chars.
            encodedString = Z85eSampleData.GetHelloString(9);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var paddedSevenByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 9, 7, Z85eSampleData.HelloWorldBytes(7));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), paddedSevenByteFinalResult);  // different

            // Decode 10 chars.
            encodedString = Z85eSampleData.GetHelloString(10);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 10, 8, Z85eSampleData.HelloWorldBytes(8));
            var helloWorldDecodedDoneResult = new DecodeExpectedData(OperationStatus.Done, 10, 8, Z85eSampleData.HelloWorldBytes(8));

            Add(Input(encodedString, false), needMoreDataResult);
            Add(Input(encodedString, true), helloWorldDecodedDoneResult);
        }

        private static DecodeInputData Input(string input, bool finalBlock) => new DecodeInputData(input, finalBlock);
    }
}
