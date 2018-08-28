using Xunit;

namespace CoenM.Encoding.Test.TestData
{
    internal class NeedMoreDataDecodeScenario : TheoryData<DecodeData>
    {
        public NeedMoreDataDecodeScenario()
        {
            // Add(new DecodeData(new string('a', 0), new byte[0], 0, 0));
            Add(new DecodeData(new string('a', 1), new byte[0], 0, 0));
            Add(new DecodeData(new string('a', 2), new byte[0], 0, 0));
            Add(new DecodeData(new string('a', 3), new byte[0], 0, 0));
            Add(new DecodeData(new string('a', 4), new byte[0], 0, 0));
            // Add(new DecodeData(new string('a', 4), new byte[0], 0, 0));
            Add(new DecodeData(new string('a', 6), new byte[4], 5, 4));
            Add(new DecodeData(new string('a', 7), new byte[4], 5, 4));
            Add(new DecodeData(new string('a', 8), new byte[4], 5, 4));
        }
    }
}