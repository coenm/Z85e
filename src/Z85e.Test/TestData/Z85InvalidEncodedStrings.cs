namespace CoenM.Encoding.Test.TestData
{
    using Xunit;

    internal class Z85InvalidEncodedStrings : TheoryData<string>
    {
        public Z85InvalidEncodedStrings()
        {
            Add(new string('a', 1));
            Add(new string('a', 2));
            Add(new string('a', 3));
            Add(new string('a', 4));
            Add(new string('a', 6));
            Add(new string('a', 6000001));
        }
    }
}
