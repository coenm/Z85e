namespace CoenM.Encoding.Test.TestData
{
    using Xunit;

    internal class Z85InvalidEncodedLengths : TheoryData<int>
    {
        public Z85InvalidEncodedLengths()
        {
            Add(1);
            Add(2);
            Add(3);
            Add(4);
            Add(6);
            Add(6000001);
        }
    }

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