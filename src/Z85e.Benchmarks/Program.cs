using Z85e.Benchmarks.Benchmarks;

namespace Z85e.Benchmarks
{
    using System.Reflection;
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Z85>(new MainConfig());

            // var summary = BenchmarkRunner.Run<Md5VsSha256>();
//            new BenchmarkSwitcher(typeof(Program).GetTypeInfo().Assembly)
//                .Run(args, new MainConfig());
        }
    }
}
