namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

    public class Z85Base64EncodeSimilarityTest
    {
        private readonly ITestOutputHelper output;

        public Z85Base64EncodeSimilarityTest(ITestOutputHelper output)
        {
            this.output = output;
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
            output.WriteLine("Z85 encode scenario:");
            output.WriteLine(z85Scenario.ToString());
            output.WriteLine(string.Empty);
            output.WriteLine("Base64 encode scenario:");
            output.WriteLine(base64Scenario.ToString());
        }

        private void WritePostTestLog(Z85Base64EncodeResult z85Result, Z85Base64EncodeResult base64Result)
        {
            output.WriteLine(string.Empty);
            output.WriteLine("---------------------------------");
            output.WriteLine(string.Empty);
            output.WriteLine("Z85 encode result:");
            output.WriteLine(z85Result.ToString());
            output.WriteLine(string.Empty);
            output.WriteLine("Base64 encode result:");
            output.WriteLine(base64Result.ToString());
        }
    }
}
