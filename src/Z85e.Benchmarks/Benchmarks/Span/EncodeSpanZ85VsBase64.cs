namespace Z85e.Benchmarks.Benchmarks.Span
{
    using System;
    using System.Buffers;

    using BenchmarkDotNet.Attributes;

    public class EncodeSpanZ85VsBase64
    {
        private byte[] data;
        private byte[] outputBytes;
        private char[] outputChars;

        [Params(120, 6000/*, 4000000*/)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[Size];
            outputBytes = new byte[Size * 2];
            outputChars = new char[Size * 2];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public OperationStatus Z85Encode()
        {
            Span<char> destination = (Size <= 120) ? stackalloc char[Size * 2] : outputChars;
            return CoenM.Encoding.Z85.Encode(data, destination, out int bytesConsumed, out int charsWritten, true);
        }

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public OperationStatus Base64Encode()
        {
            Span<byte> destination = (Size <= 120) ? stackalloc byte[Size * 2] : outputBytes;
            return System.Buffers.Text.Base64.EncodeToUtf8(data, destination, out int bytesConsumed, out int bytesWritten, true);
        }
    }
}
