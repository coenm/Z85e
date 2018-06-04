using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Z85e.Benchmarks.Benchmarks
{
    public class DecodeZ85VsBase64
    {
        private string _base64String;
        private string _z85String;

        [Params(120, 6000)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var data = new byte[Size];
            new Random(42).NextBytes(data);
            _base64String = Convert.ToBase64String(data);
            _z85String = CoenM.Encoding.Z85.Encode(data);
        }

        [Benchmark]
        public IEnumerable<byte> Z85Decode() => CoenM.Encoding.Z85.Decode(_z85String);

        [Benchmark]
        public byte[] Base64Decode() => Convert.FromBase64String(_base64String);
    }
}