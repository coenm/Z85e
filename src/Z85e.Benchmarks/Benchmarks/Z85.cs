using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;

namespace Z85e.Benchmarks.Benchmarks
{
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    [ClrJob, CoreJob]
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
    [MemoryDiagnoser]
    [RankColumn]
    public class Z85
    {
        private Memory<byte> data;

        [GlobalSetup]
        public void GlobalSetup()
        {
            data = new byte[10]; // executed once per each N value
        }

        public string EncodedText { get; } = new String('a', 10);

        [Benchmark(Baseline = true)]
        public void DecodeString() => CoenM.Encoding.Z85.Decode(EncodedText);

        [Benchmark]
        public void DecodeStringMemoryField() => CoenM.Encoding.Z85.Decode(EncodedText.AsSpan(), data.Span);

        [Benchmark]
        public void DecodeStringSpanVariable()
        {
            Span<byte> span = new byte[10];
            CoenM.Encoding.Z85.Decode(EncodedText.AsSpan(), span);
        }

        [Benchmark]
        public void DecodeStringSpan2Variable()
        {
            Span<byte> span = stackalloc byte[10];
            CoenM.Encoding.Z85.Decode(EncodedText.AsSpan(), span);
        }
    }
}