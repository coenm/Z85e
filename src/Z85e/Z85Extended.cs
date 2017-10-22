using System;
using System.Collections.Generic;
using System.Linq;

namespace CoenM.Z85e
{
    public static class Z85Extended
    {
        public static IEnumerable<byte> Decode(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var len = (uint)input.Length;

            if (len % 5 == 0)
                return Z85.Decode(input);

            if (len % 5 == 1)
            {
                char x = input[(int) (len - 1)];
                int y = Int32.Parse(x.ToString());
                var result = Z85.Decode(input.Substring(0, (int) (len-1)));
                return result.Take(result.Count() - y);
            }

            throw new ArgumentOutOfRangeException(nameof(input), "Length of Input should be multiple of 5 or a multiple of 5 + 1.");
        }

        public static string Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var dataLength = (uint)data.Length;
            
            var size = dataLength % 4;
            if (size == 0)
                return Z85.Encode(data);

            var newDataList = data.Concat(new byte[4-size]).ToArray();
            var result = Z85.Encode(newDataList);

            return $"{result}{4 - size}";
        }
    }
}