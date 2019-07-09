﻿namespace Z85e.Benchmarks
{
    using BenchmarkDotNet.Running;
    using Z85e.Benchmarks.Benchmarks;
    using Z85e.Benchmarks.Benchmarks.Span;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(EncodeZ85VsBase64),
                typeof(DecodeZ85VsBase64),
                typeof(EncodeZ85ExtVsBase64),
                typeof(DecodeZ85ExtVsBase64),
                typeof(EncodeSpanZ85VsBase64),
                typeof(DecodeSpanZ85VsBase64),
            });
            switcher.Run(args, new MainConfig());
        }
    }
}
