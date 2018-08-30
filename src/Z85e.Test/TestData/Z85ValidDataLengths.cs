namespace CoenM.Encoding.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    using Xunit;

    /// <summary>
    /// Z85 only encoded blocks of 4 bytes. All sizes divisible by 4 are valid.
    /// </summary>
    internal class Z85ValidDataLengths : TheoryData<int>
    {
        public Z85ValidDataLengths()
        {
            Add(0);
            Add(4);
            Add(8);
            Add(4000000);
        }
    }
}