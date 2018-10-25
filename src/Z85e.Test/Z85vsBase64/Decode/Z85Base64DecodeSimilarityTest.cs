namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

    public class Z85Base64DecodeSimilarityTest
    {
        private readonly ITestOutputHelper output;

        public Z85Base64DecodeSimilarityTest(ITestOutputHelper output)
        {
            this.output = output;
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
            output.WriteLine("Z85 decode scenario:");
            output.WriteLine(z85Scenario.ToString());
            output.WriteLine(string.Empty);
            output.WriteLine("Base64 decode scenario:");
            output.WriteLine(base64Scenario.ToString());
        }

        private void WritePostTestLog(Z85Base64DecodeResult z85Result, Z85Base64DecodeResult base64Result)
        {
            output.WriteLine(string.Empty);
            output.WriteLine("---------------------------------");
            output.WriteLine(string.Empty);
            output.WriteLine("Z85 decode result:");
            output.WriteLine(z85Result.ToString());
            output.WriteLine(string.Empty);
            output.WriteLine("Base64 decode result:");
            output.WriteLine(base64Result.ToString());
        }
    }
}
