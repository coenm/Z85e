using FluentAssertions;
using Xunit;

namespace CoenM.Encoding.Test.Z85vsBase64
{
    public class Z85Base64SimilarityTest
    {
        [Theory]
        [ClassData(typeof(Z85Base64EncodeScenarios))]
        public void Z85Base64DecodeSimilarityTest(Z85DecodeScenario z85Scenario, Base64DecodeScenario base64Scenario)
        {
            var resultZ85 = z85Scenario.Decode();
            var resultB64 = base64Scenario.Decode();

            resultZ85.Should().BeEquivalentTo(resultB64);
        }
    }
}