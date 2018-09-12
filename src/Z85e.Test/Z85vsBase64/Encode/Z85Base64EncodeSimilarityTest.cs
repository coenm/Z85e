using FluentAssertions;
using Xunit;

namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    public class Z85Base64EncodeSimilarityTest
    {
        [Theory]
        [ClassData(typeof(Z85Base64EncodeScenarios))]
        public void Test(Z85EncodeScenario z85Scenario, Base64EncodeScenario base64Scenario)
        {
            var resultZ85 = z85Scenario.Encode();
            var resultB64 = base64Scenario.Encode();

            resultZ85.Should().BeEquivalentTo(resultB64);
        }
    }
}