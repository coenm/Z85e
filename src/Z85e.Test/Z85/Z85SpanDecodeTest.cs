// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable All
namespace CoenM.Encoding.Test.Z85
{
    using System;

    using TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85SpanDecodeTest
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
                scenario.Mode,
                scenario.IsFinalBlock);

            // assert
            expectedResult.AssertResult(result, charsConsumed, bytesWritten, destination);
        }
    }
}