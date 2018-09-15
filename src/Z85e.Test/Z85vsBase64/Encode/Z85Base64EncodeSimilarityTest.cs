using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    public class Z85Base64EncodeSimilarityTest
    {
        private readonly ITestOutputHelper _output;

        public Z85Base64EncodeSimilarityTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(Z85Base64EncodeScenarios))]
        public void Test(Z85EncodeScenario z85Scenario, Base64EncodeScenario base64Scenario)
        {
            // arrange
            WritePreTestLog(z85Scenario, base64Scenario);

            // act
            var resultZ85 = z85Scenario.Encode();
            var resultB64 = base64Scenario.Encode();

            // assert
            WritePostTestLog(resultZ85, resultB64);
            resultZ85.Should().BeEquivalentTo(resultB64);
        }

        private void WritePreTestLog(Z85EncodeScenario z85Scenario, Base64EncodeScenario base64Scenario)
        {
            _output.WriteLine("Z85 encode scenario:");
            _output.WriteLine(z85Scenario.ToString());
            _output.WriteLine(string.Empty);
            _output.WriteLine("Base64 encode scenario:");
            _output.WriteLine(base64Scenario.ToString());
        }

        private void WritePostTestLog(Z85Base64EncodeResult z85Result, Z85Base64EncodeResult base64Result)
        {
            _output.WriteLine(string.Empty);
            _output.WriteLine("---------------------------------");
            _output.WriteLine(string.Empty);
            _output.WriteLine("Z85 encode result:");
            _output.WriteLine(z85Result.ToString());
            _output.WriteLine(string.Empty);
            _output.WriteLine("Base64 encode result:");
            _output.WriteLine(base64Result.ToString());
        }
    }
}