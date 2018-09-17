namespace CoenM.Encoding.Test.TestData
{
    using Xunit;

    /// <summary>
    /// Z85 only encoded blocks of 4 bytes. All sizes not divisible by 4 are invalid.
    /// </summary>
    internal class Z85InvalidDataLengths : TheoryData<int>
    {
        public Z85InvalidDataLengths()
        {
            Add(1);
            Add(2);
            Add(3);
            Add(5);
            Add(6);
            Add(6000001);
        }
    }
}