using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Z85e.Benchmarks.Benchmarks
{
    public class DecodeZ85ExtVsBase64
    {
        private string _base64String;
        private string _z85String;

        [Params(122, 123, 124, 125)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var data = new byte[Size];
            new Random(42).NextBytes(data);
            _base64String = Convert.ToBase64String(data);
            _z85String = CoenM.Encoding.Z85Extended.Encode(data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public IEnumerable<byte> Z85ExtendedDecode() => CoenM.Encoding.Z85Extended.Decode(_z85String);

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public byte[] Base64Decode() => Convert.FromBase64String(_base64String);
    }
}