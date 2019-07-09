namespace Z85e.Benchmarks.Benchmarks
{
    using System;

    using BenchmarkDotNet.Attributes;

    public class EncodeZ85ExtVsBase64
    {
        private byte[] data;

        [Params(122, 123, 124, 125)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[Size];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public string Z85ExtendedEncode() => CoenM.Encoding.Z85Extended.Encode(data);

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public string Base64Encode() => Convert.ToBase64String(data);
    }
}
