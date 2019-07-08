namespace CoenM.Encoding.Test.Generator
{
    using System.IO;
    using System.Linq;
    using System.Text;

    using Xunit;
    using Xunit.Abstractions;

    public class MapGeneration
    {
        private readonly ITestOutputHelper output;

        public MapGeneration(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GenerateDecoderTableTest()
        {
            var s = new StringBuilder();

            var result = new byte[256];
            for (int ij = 0; ij < result.Length; ij++)
            {
                result[ij] = 0x00;
            }

            var index = 0;
            foreach (var c in Internals.Map.Encoder)
            {
                result[c] = (byte)index;
                index++;
            }

            //
            //            int i = 0;
            //            s.AppendLine("internal static readonly byte[] Decoder =");
            //            s.AppendLine("{");
            //            foreach (var b in result)
            //            {
            //                i++;
            //                s.Append($"0x{b:X2}, ");
            //
            //                if (i % 8 == 0)
            //                    s.AppendLine(string.Empty);
            //            }
            //
            //            s.AppendLine("};");

            var sw = new StringWriter(s);
            GenerateDecoderTable(result, 32, 128, 8, sw);
            output.WriteLine(sw.ToString());
        }

        private void GenerateDecoderTable(byte[] bytes, int offset, int count, int lineBreak, StringWriter s)
        {
            int i = 0;
            s.WriteLine("internal static readonly byte[] Decoder =");
            s.WriteLine("{");
            foreach (var b in bytes.Skip(offset).Take(count))
            {
                i++;
                s.Write($"0x{b:X2}, ");

                if (i % lineBreak == 0)
                    s.WriteLine(string.Empty);
            }

            s.WriteLine("};");
        }
    }
}
