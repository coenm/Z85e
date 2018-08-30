// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable All
namespace CoenM.Encoding.Test.Z85
{
    using System;
    using System.Buffers;

    using TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85SpanDecodeTest
    {
        [Theory]
        [ClassData(typeof(ModeAndFinalBlockOptions))]
        public void DecodeEmptyTest(Z85Mode mode, bool isFinalBlock)
        {
            // arrange
            Span<char> source = Span<char>.Empty;
            Span<byte> destination = new byte[100];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, mode, isFinalBlock);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(0, charsConsumed);
            Assert.Equal(0, bytesWritten);
        }

        [Theory]
        [ClassData(typeof(NeedMoreDataDecodeScenario))]
        public void BasicDecodingWithFinalBlockFalseAndPaddingModeWithKnownInputNeedMoreDataTest(DecodeData scenario)
        {
            // arrange
            ReadOnlySpan<char> source = scenario.EncodedString.Span;
            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, Z85Mode.Padding, isFinalBlock: false);

            // assert
            Assert.Equal(OperationStatus.NeedMoreData, result);
            Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
            Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
            Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        }

        [Theory]
        [ClassData(typeof(NeedMoreDataDecodeScenario))]
        public void BasicDecodingWithFinalBlockFalseAndModeStrictKnownInputNeedMoreDataTest(DecodeData scenario)
        {
            // arrange
            ReadOnlySpan<char> source = scenario.EncodedString.Span;
            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, Z85Mode.Strict, isFinalBlock: false);

            // assert
            Assert.Equal(OperationStatus.NeedMoreData, result);
            Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
            Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
            Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        }

        [Theory]
        [ClassData(typeof(NeedMoreDataDecodeScenario))]
        public void BasicDecodingWithFinalBlockTrueAndPaddingModeStrictKnownInputReturnsInvalidDataTest(DecodeData scenario)
        {
            // arrange
            ReadOnlySpan<char> source = scenario.EncodedString.Span;
            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, Z85Mode.Strict, isFinalBlock: true);

            // assert
            Assert.Equal(OperationStatus.InvalidData, result);
            Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
            Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
            Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        }

        [Theory]
        [ClassData(typeof(PaddedZ85Scenario))]
        public void BasicDecodingWithFinalBlockTrueAndPaddingModePaddingKnownInputReturnsPaddedOutputTest(DecodeData scenario)
        {
            // arrange
            ReadOnlySpan<char> source = scenario.EncodedString.Span;
            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, Z85Mode.Padding, isFinalBlock: true);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            //todo
            // Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
            // Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
            // Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        }


        [Theory]
        [ClassData(typeof(NeedMoreDataDecodeScenarioC))]
        public void BasicDecodingWithFinalBlockTrueAndPaddingModePaddingKnownInputWithWrongSizeReturnsInvalidDataTest(DecodeData scenario)
        {
            // arrange
            ReadOnlySpan<char> source = scenario.EncodedString.Span;
            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, Z85Mode.Padding, isFinalBlock: true);

            // assert
            Assert.Equal(OperationStatus.InvalidData, result);
            Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
            Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
            Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        }


        //        [Theory]
        //        public void BasicDecodingWithFinalBlockFalseKnownInputInvalidTest(DecodeData scenario)
        //        {
        //            // arrange
        //            ReadOnlySpan<char> source = scenario.EncodedString.Span;
        //            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];
        //
        //            // act
        //            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, isFinalBlock: false);
        //
        //            // assert
        //            Assert.Equal(OperationStatus.InvalidData, result);
        //            Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
        //            Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
        //            Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        //        }

        // [Theory]
        // todo fix data
        //        public void BasicDecodingWithFinalBlockTrueKnownInputInvalidTest(DecodeData scenario)
        //        {
        //            // arrange
        //            ReadOnlySpan<char> source = scenario.EncodedString.Span;
        //            Span<byte> destination = new byte[scenario.ExpectedBytesWritten + 20];
        //
        //            // act
        //            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, isFinalBlock: true);
        //
        //            // assert
        //            Assert.Equal(OperationStatus.InvalidData, result);
        //            Assert.Equal(scenario.ExpectedCharactersConsumed, charsConsumed);
        //            Assert.Equal(scenario.ExpectedBytesWritten, bytesWritten);
        //            Assert.Equal(scenario.ExpectedData, destination.Slice(0, bytesWritten).ToArray());
        //        }

        [Theory]
        [ClassData(typeof(StrictZ85Samples))]
        public void BasicDecodingWithFinalBlockTrueKnownInputDoneTest(byte[] data, string encoded)
        {
            // arrange
            ReadOnlySpan<char> source = encoded.AsSpan();
            Span<byte> destination = new byte[data.Length + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, isFinalBlock: true);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(encoded.Length, charsConsumed);
            Assert.Equal(data.Length, bytesWritten);
            Assert.Equal(data, destination.Slice(0, bytesWritten).ToArray());
        }

        [Theory]
        [ClassData(typeof(StrictZ85Samples))]
        public void BasicDecodingWithFinalBlockFalseKnownInputDoneTest(byte[] data, string encoded)
        {
            // arrange
            ReadOnlySpan<char> source = encoded.AsSpan();
            Span<byte> destination = new byte[data.Length + 20];

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten, isFinalBlock: false);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(encoded.Length, charsConsumed);
            Assert.Equal(data.Length, bytesWritten);
            Assert.Equal(data, destination.Slice(0, bytesWritten).ToArray());
        }


        //        [Theory]
        //        [ClassData(typeof(Z85InvalidEncodedStrings))]
        //        public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(string encoded)
        //        {
        //            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Decode(encoded));
        //        }

        // [Fact]
        // public void CalculateDecodedSizeTest()
        // {
        //     var result = Sut.CalculateDecodedSize(HelloWorldString.AsSpan());
        //     Assert.Equal(_helloWorldBytes.Length, result);
        // }

        [Fact]
        public void CalculateDecodedSizeThrowsExceptionWhenInputHasWrongSizeTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Sut.CalculateDecodedSize(null));
        }





        // [Theory]
        // [ClassData(typeof(Z85InvalidEncodedLengths))]
        // public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(int size)
        // {
        //     Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Decode(new string('a', size)));
        // }
    }
}