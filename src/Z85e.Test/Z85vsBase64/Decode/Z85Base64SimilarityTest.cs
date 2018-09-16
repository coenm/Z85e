using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    public class Z85Base64DecodeSimilarityTest
    {
        private readonly ITestOutputHelper _output;

        public Z85Base64DecodeSimilarityTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(Z85Base64DecodeScenarios))]
        public void Test(Z85DecodeScenario z85Scenario, Base64DecodeScenario base64Scenario)
        {
            // arrange
            WritePreTestLog(z85Scenario, base64Scenario);

            // act
            var resultZ85 = z85Scenario.Decode();
            var resultB64 = base64Scenario.Decode();

            // assert
            WritePostTestLog(resultZ85, resultB64);
            resultZ85.Should().BeEquivalentTo(resultB64);
        }

        private void WritePreTestLog(Z85DecodeScenario z85Scenario, Base64DecodeScenario base64Scenario)
        {
            _output.WriteLine("Z85 decode scenario:");
            _output.WriteLine(z85Scenario.ToString());
            _output.WriteLine(string.Empty);
            _output.WriteLine("Base64 decode scenario:");
            _output.WriteLine(base64Scenario.ToString());
        }

        private void WritePostTestLog(Z85Base64DecodeResult z85Result, Z85Base64DecodeResult base64Result)
        {
            _output.WriteLine(string.Empty);
            _output.WriteLine("---------------------------------");
            _output.WriteLine(string.Empty);
            _output.WriteLine("Z85 decode result:");
            _output.WriteLine(z85Result.ToString());
            _output.WriteLine(string.Empty);
            _output.WriteLine("Base64 decode result:");
            _output.WriteLine(base64Result.ToString());
        }
    }
}