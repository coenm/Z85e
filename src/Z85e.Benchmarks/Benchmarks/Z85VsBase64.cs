using System;
using BenchmarkDotNet.Attributes;

namespace Z85e.Benchmarks.Benchmarks
{
    public class EncodeZ85VsBase64
    {
        private byte[] _data;

        [Params(120, 6000/*, 4000000*/)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _data = new byte[Size];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        public int Z85Encode()
        {
            var readOnlySpan = _data.AsSpan();
            Span<char> result = new char[CoenM.Encoding.Z85.CalcuateEncodedSize(readOnlySpan)];

            return CoenM.Encoding.Z85.Encode(readOnlySpan, result);
        }

        [Benchmark]
        public string Base64Encode1() => Convert.ToBase64String(_data);

        [Benchmark]
        public void Base64Encode2()
        {
            var output = new char[Size*2];
            Convert.ToBase64CharArray(_data, 0, Size, output, 0);
        }
    }
}