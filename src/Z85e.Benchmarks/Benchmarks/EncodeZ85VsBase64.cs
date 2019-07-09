namespace Z85e.Benchmarks.Benchmarks
{
    using System;

    using BenchmarkDotNet.Attributes;

    public class EncodeZ85VsBase64
    {
        private byte[] data;

        [Params(120, 6000/*, 4000000*/)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[Size];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public string Z85Encode() => CoenM.Encoding.Z85.Encode(data);

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public string Base64Encode() => Convert.ToBase64String(data);
    }
}
