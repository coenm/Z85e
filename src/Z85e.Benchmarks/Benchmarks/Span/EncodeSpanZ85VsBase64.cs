using System;
using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace Z85e.Benchmarks.Benchmarks.Span
{
    public class EncodeSpanZ85VsBase64
    {
        private byte[] _data;
        private byte[] _outputBytes;
        private char[] _outputChars;


        [Params(120, 6000/*, 4000000*/)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _data = new byte[Size];
            _outputBytes = new byte[Size * 2];
            _outputChars = new char[Size * 2];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        [BenchmarkCategory("Z85")]
        public OperationStatus Z85Encode()
        {
            Span<char> destination = (Size <= 120) ? stackalloc char[Size * 2] : _outputChars;
            return CoenM.Encoding.Z85.Encode(_data, destination, out int bytesConsumed, out int charsWritten, true);
        }

        [Benchmark]
        [BenchmarkCategory("Base64")]
        public OperationStatus Base64Encode()
        {
            Span<byte> destination = (Size <= 120) ? stackalloc byte[Size * 2] : _outputBytes;
            return System.Buffers.Text.Base64.EncodeToUtf8(_data, destination, out int bytesConsumed, out int bytesWritten, true);
        }
    }
}