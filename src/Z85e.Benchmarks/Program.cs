using Z85e.Benchmarks.Benchmarks;

namespace Z85e.Benchmarks
{
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
//            BenchmarkRunner.Run<EncodeZ85VsBase64>(new MainConfig());

            var switcher = new BenchmarkSwitcher(new[] {
                typeof(EncodeZ85VsBase64),
                typeof(DecodeZ85VsBase64),
            });
            switcher.Run(args, new MainConfig());
        }
    }
}