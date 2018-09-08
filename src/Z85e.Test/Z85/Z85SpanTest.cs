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
    }
}