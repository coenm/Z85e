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
        [BenchmarkCategory("Z85")]
        public string Z85Encode() => CoenM.Encoding.Z85.Encode(_data);

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public string Base64Encode() => Convert.ToBase64String(_data);
    }
}