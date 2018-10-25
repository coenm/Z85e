namespace Z85e.Benchmarks.Benchmarks
{
    using System;
    using System.Collections.Generic;

    using BenchmarkDotNet.Attributes;

    public class DecodeZ85VsBase64
    {
        private string base64String;
        private string z85String;

        [Params(120, 6000)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var data = new byte[Size];
            new Random(42).NextBytes(data);
            base64String = Convert.ToBase64String(data);
            z85String = CoenM.Encoding.Z85.Encode(data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public IEnumerable<byte> Z85Decode() => CoenM.Encoding.Z85.Decode(z85String);

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public byte[] Base64Decode() => Convert.FromBase64String(base64String);
    }
}
