using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.CsProj;

namespace Z85e.Benchmarks
{
    /// <remarks>
    /// Based on <see cref="https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1/"/>
    /// </remarks>
    public class MainConfig : ManualConfig
    {
        public MainConfig()
        {
            // Only x64
            var defaultJob = Job.ShortRun.With(Platform.X64);

            // Toolchain
            // Add(defaultJob.With(CsProjClassicNetToolchain.Net461));
            Add(defaultJob.With(CsProjClassicNetToolchain.Net471));
            Add(defaultJob.With(CsProjCoreToolchain.NetCoreApp20));
            Add(defaultJob.With(CsProjCoreToolchain.NetCoreApp21));

            // Extra diagnoser
            Add(MemoryDiagnoser.Default);

            // Columns
            // Add(DefaultColumnProviders.Instance);
            Add(new MinimalColumnProvider());
            Add(new ParamsColumnProvider());
            Add(new DiagnosersColumnProvider());
            // Add(RankColumn.Arabic);

            // Ordering
            // Set(new DefaultOrderProvider(SummaryOrderPolicy.SlowestToFastest));

            // Grouping
            Add(BenchmarkLogicalGroupRule.ByCategory);

            // Exporters
            Add(MarkdownExporter.GitHub);
            // Add(CsvMeasurementsExporter.Default);
            // Add(RPlotExporter.Default);

            // Logger
            Add(new ConsoleLogger());
        }

        private sealed class MinimalColumnProvider : IColumnProvider
        {
            public IEnumerable<IColumn> GetColumns(Summary summary)
            {
                yield return CategoriesColumn.Default;
                yield return TargetMethodColumn.Method;
                yield return new JobCharacteristicColumn(InfrastructureMode.ToolchainCharacteristic);
                yield return StatisticColumn.Mean;
            }
        }

        /// <remarks>taken from https://github.com/dotnet/BenchmarkDotNet/blob/master/src/BenchmarkDotNet/Columns/DefaultColumnProvider.cs</remarks>>
        private class ParamsColumnProvider : IColumnProvider
        {
            public IEnumerable<IColumn> GetColumns(Summary summary) => summary
                .Benchmarks
                .SelectMany(b => b.Parameters.Items.Select(item => item.Name))
                .Distinct()
                .Select(name => new ParamColumn(name));
        }

        /// <remarks>taken from https://github.com/dotnet/BenchmarkDotNet/blob/master/src/BenchmarkDotNet/Columns/DefaultColumnProvider.cs</remarks>>
        private class DiagnosersColumnProvider : IColumnProvider
        {
            public IEnumerable<IColumn> GetColumns(Summary summary) => summary
                .Config
                .GetDiagnosers()
                .Select(d => d.GetColumnProvider())
                .SelectMany(cp => cp.GetColumns(summary));
        }
    }
}