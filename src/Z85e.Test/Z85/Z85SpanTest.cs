using FluentAssertions;

namespace CoenM.Encoding.Test.Z85
{
    using System;

    using TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85SpanTest
    {
        [Theory]
        [ClassData(typeof(AllDecodeScenarios))]
        public void DecodeTest(DecodeInputData scenario, DecodeExpectedData expectedResult)
        {
            // arrange
            Span<byte> destination = scenario.CreateDestination().Span;

            // act
            var result = Sut.Decode(
                scenario.Source.Span,
                destination,
                out var charsConsumed,
                out var bytesWritten,
                scenario.IsFinalBlock);

            // assert
            expectedResult.AssertResult(result, charsConsumed, bytesWritten, destination);
        }


        [Theory]
        [ClassData(typeof(AllEncodeScenarios))]
        public void EncodeTest(EncodeInputData scenario, EncodeExpectedData expectedResult)
        {
            // arrange
            Span<char> destination = scenario.CreateDestination().Span;

            // act
            var result = Sut.Encode(
                scenario.Source.Span,
                destination,
                out var bytesConsumed,
                out var charsWritten,
                scenario.IsFinalBlock);

            // assert
            expectedResult.AssertResult(result, bytesConsumed, charsWritten, destination);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)] // one remaining character cannot be decoded.
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 3)]
        [InlineData(5, 4)]
        [InlineData(6, 4)] // one remaining character cannot be decoded.
        [InlineData(7, 5)]
        [InlineData(8, 6)]
        [InlineData(9, 7)]
        [InlineData(10, 8)]
        public void GetDecodedSizeTest(int inputSize, int expectedOutput)
        {
            // arrange
            Span<char> input = new char[inputSize];

            // act
            var result = Sut.GetDecodedSize(input);

            // assert
            result.Should().Be(expectedOutput);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(-108)]
        public void GetDecodedSize_ThrowsException_WhenInputSizeIsNegativeTest(int inputSize)
        {
            // arrange

            // act
            Action act = () => Sut.GetDecodedSize(inputSize);

            // assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 4)]
        [InlineData(4, 5)]
        [InlineData(5, 7)]
        [InlineData(6, 8)]
        [InlineData(7, 9)]
        [InlineData(8, 10)]
        [InlineData(9, 12)]
        [InlineData(10, 13)]
        public void GetEncodedSizeTest(int inputSize, int expectedOutput)
        {
            // arrange
            Span<byte> input = new byte[inputSize];

            // act
            var result = Sut.GetEncodedSize(input);

            // assert
            result.Should().Be(expectedOutput);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(-123)]
        public void GetEncodedSize_ThrowsException_WhenInputSizeIsNegativeTest(int inputSize)
        {
            // arrange

            // act
            Action act = () => Sut.GetEncodedSize(inputSize);

            // assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}