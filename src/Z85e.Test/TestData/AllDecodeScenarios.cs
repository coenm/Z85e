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

            Add(Input(encodedString, Z85Mode.Padding, false), doneResult);
            Add(Input(encodedString, Z85Mode.Padding, true), doneResult);
            Add(Input(encodedString, Z85Mode.Strict, false), doneResult);
            Add(Input(encodedString, Z85Mode.Strict, true), doneResult);


            // Decode 1 character.
            // One character is special and can never be decoded.
            var needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 0, 0, new byte[0]);
            var invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 0, 0, new byte[0]);
            encodedString = Z85eSampleData.GetHelloString(1);

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), invalidDataResult);
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 2 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(2);
            var paddedOneByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 2, 1, Z85eSampleData.HelloWorldBytes(1));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedOneByteFinalResult); // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 3 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(3);
            var paddedTwoByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 3, 2, Z85eSampleData.HelloWorldBytes(2));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedTwoByteFinalResult); // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 4 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(4);
            var paddedThreeByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 4, 3, Z85eSampleData.HelloWorldBytes(3));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedThreeByteFinalResult); // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 5 chars.
            // This is allowed in strict Z85
            // In all cases the result should be the same.
            //
            encodedString = Z85eSampleData.GetHelloString(5);
            var helloDecodedDoneResult = new DecodeExpectedData(OperationStatus.Done, 5, 4, Z85eSampleData.HelloWorldBytes(4));

            Add(Input(encodedString, Z85Mode.Padding, false), helloDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Padding, true), helloDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, false), helloDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, true), helloDecodedDoneResult);


            //
            // Decode 6 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(6);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, Z85eSampleData.HelloWorldBytes(4));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), invalidDataResult);
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);


            //
            // Decode 7 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(7);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var paddedFiveByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 7, 5, Z85eSampleData.HelloWorldBytes(5));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedFiveByteFinalResult);  // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);

            //
            // Decode 8 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(8);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var paddedSixByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 8, 6, Z85eSampleData.HelloWorldBytes(6));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedSixByteFinalResult);  // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);


            //
            // Decode 9 chars.
            //
            encodedString = Z85eSampleData.GetHelloString(9);
            needMoreDataResult = new DecodeExpectedData(OperationStatus.NeedMoreData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            invalidDataResult = new DecodeExpectedData(OperationStatus.InvalidData, 5, 4, Z85eSampleData.HelloWorldBytes(4));
            var paddedSevenByteFinalResult = new DecodeExpectedData(OperationStatus.Done, 9, 7, Z85eSampleData.HelloWorldBytes(7));

            Add(Input(encodedString, Z85Mode.Padding, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Padding, true), paddedSevenByteFinalResult);  // different
            Add(Input(encodedString, Z85Mode.Strict, false), needMoreDataResult);
            Add(Input(encodedString, Z85Mode.Strict, true), invalidDataResult);


            //
            // Decode 10 chars.
            // This is allowed in strict Z85
            // In all cases the result should be the same.
            //
            encodedString = Z85eSampleData.GetHelloString(10);
            var helloWorldDecodedDoneResult = new DecodeExpectedData(OperationStatus.Done, 10, 8, Z85eSampleData.HelloWorldBytes(8));

            Add(Input(encodedString, Z85Mode.Padding, false), helloWorldDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Padding, true), helloWorldDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, false), helloWorldDecodedDoneResult);
            Add(Input(encodedString, Z85Mode.Strict, true), helloWorldDecodedDoneResult);
        }

        private static DecodeInputData Input(string input, Z85Mode mode, bool finalBlock) => new DecodeInputData(input, mode, finalBlock);
    }
}