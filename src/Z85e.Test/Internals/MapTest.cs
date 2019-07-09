namespace CoenM.Encoding.Test.Internals
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Xunit;
    using Xunit.Abstractions;

    public class MapTest
    {
        private readonly ITestOutputHelper output;

        public MapTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SutDecoderShouldCorrespondWithEncoderTable()
        {
            var sutDecoder = Encoding.Internals.Map.Decoder;
            var generatedDecoder = GenerateDecoder(0, 128).ToArray();
            Assert.Equal(sutDecoder, generatedDecoder);
        }

        [Fact]
        public void GenerateDecoderTableTest()
        {
            var decoder = GenerateDecoder();
            var sw = new StringWriter();
            GenerateDecoderTableInCSharp(decoder, 0, 128, 8, sw);
            output.WriteLine(sw.ToString());
        }

        private IEnumerable<byte> GenerateDecoder(int offset = 0, int count = 0)
        {
            var table = new byte[256];
            var index = 0;

            foreach (var c in Encoding.Internals.Map.Encoder)
            {
                table[c] = (byte)index;
                index++;
            }

            var result = table.AsEnumerable().Skip(offset);
            if (count > 0)
                result = result.Take(count);

            return result;
        }

        private void GenerateDecoderTableInCSharp(IEnumerable<byte> bytes, int offset, int count, int lineBreak, TextWriter s)
        {
            var i = 0;
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
