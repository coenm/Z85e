using FluentAssertions;
using Xunit;

namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    public class Z85Base64DecodeSimilarityTest
    {
        [Theory]
        [ClassData(typeof(Z85Base64DecodeScenarios))]
        public void Test(Z85DecodeScenario z85Scenario, Base64DecodeScenario base64Scenario)
        {
            var resultZ85 = z85Scenario.Decode();
            var resultB64 = base64Scenario.Decode();

            resultZ85.Should().BeEquivalentTo(resultB64);
        }
    }
}