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
}