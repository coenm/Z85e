using System;
using System.Buffers;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Z85e.Benchmarks.Benchmarks.Span
{
    public class DecodeSpanZ85VsBase64
    {
        private byte[] _outputBytes;
        private string _z85String;
        private byte[] _base64Bytes;

        [Params(120, 6000/*, 4000000*/)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var data = new byte[Size];
            new Random(42).NextBytes(data);
            _outputBytes = new byte[Size * 2];
            _base64Bytes = Encoding.UTF8.GetBytes(Convert.ToBase64String(data));
            _z85String = CoenM.Encoding.Z85.Encode(data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public OperationStatus Z85Decode()
        {
            Span<byte> destination = (Size <= 120) ? stackalloc byte[Size * 2] : _outputBytes;
            return CoenM.Encoding.Z85.Decode(_z85String.AsSpan(), destination, out int bytesConsumed, out int charsWritten, true);
        }

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public OperationStatus Base64Decode()
        {
            Span<byte> destination = (Size <= 120) ? stackalloc byte[Size * 2] : _outputBytes;
            return System.Buffers.Text.Base64.DecodeFromUtf8(_base64Bytes, destination, out int bytesConsumed, out int bytesWritten, true);
        }
    }

}