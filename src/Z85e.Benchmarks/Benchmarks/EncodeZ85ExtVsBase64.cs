using System;
using BenchmarkDotNet.Attributes;

namespace Z85e.Benchmarks.Benchmarks
{
    public class EncodeZ85ExtVsBase64
    {
        private byte[] _data;

        [Params(122, 123, 124, 125)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _data = new byte[Size];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public string Z85ExtendedEncode() => CoenM.Encoding.Z85Extended.Encode(_data);

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public string Base64Encode() => Convert.ToBase64String(_data);
    }
}