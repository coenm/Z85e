using System.Collections.Generic;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.CsProj;

namespace Z85e.Benchmarks
{
    // https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1/
    public class MainConfig : ManualConfig
    {
        public MainConfig()
        {
            var defaultJob = Job.ShortRun.With(Platform.X64);
//            Add(defaultJob.With(CsProjClassicNetToolchain.Net461));
            Add(defaultJob.With(CsProjClassicNetToolchain.Net471));
            Add(defaultJob.With(CsProjCoreToolchain.NetCoreApp20));
            Add(defaultJob.With(CsProjCoreToolchain.NetCoreApp21));


            Add(MemoryDiagnoser.Default);

            Add(DefaultColumnProviders.Instance);
//          Add(new MinimalColumnProvider());
            Add(MemoryDiagnoser.Default.GetColumnProvider());
            Add(RankColumn.Arabic);
            Set(new DefaultOrderProvider(SummaryOrderPolicy.SlowestToFastest));

            // exporters
            Add(MarkdownExporter.GitHub);
            Add(CsvMeasurementsExporter.Default);
            Add(RPlotExporter.Default);

            // logger
            Add(new ConsoleLogger());
        }

//        private sealed class MinimalColumnProvider : IColumnProvider
//        {
//            public IEnumerable<IColumn> GetColumns(Summary summary)
//            {
//                yield return TargetMethodColumn.Method;
//                yield return new JobCharacteristicColumn(InfrastructureMode.ToolchainCharacteristic);
//                yield return StatisticColumn.Mean;
//            }
//        }
    }
}