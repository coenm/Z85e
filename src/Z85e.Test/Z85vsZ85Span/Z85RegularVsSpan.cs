namespace CoenM.Encoding.Test.Z85vsZ85Span
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using TestInternals;

    using FluentAssertions;

    using JetBrains.Annotations;

    using Xunit;

    using Z85 = Encoding.Z85;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "XUnit requires public properties to be used as data providers for tests.")]
    public class Z85RegularVsSpan
    {
        public static IEnumerable<object[]> Data
        {
            [UsedImplicitly]
            get
            {
                foreach (var seed in new[] { 42, 54, 550 })
                {
                    yield return new object[] { seed, 233 };
                    yield return new object[] { seed, 234 };
                    yield return new object[] { seed, 235 };
                    yield return new object[] { seed, 236 };
                    yield return new object[] { seed, 237 };
                    yield return new object[] { seed, 238 };
                    yield return new object[] { seed, 239 };
                    yield return new object[] { seed, 229 };
                    yield return new object[] { seed, 801 };
                    yield return new object[] { seed, 8101 };
                    yield return new object[] { seed, 1231 };
                }
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Z85RegularShouldHaveSameResultsAsSpanVersionTest(int seed, uint byteLength)
        {
            // arrange
            var data = new Span<byte>(MyData.CreatePseudoRandomByteArray(byteLength, seed));
            var outputEncode = new Span<char>(new char[byteLength * 2]);
            var outputDecode = new Span<byte>(new byte[byteLength]);

            // act
            var resultEncodeRegular = Z85Extended.Encode(data.ToArray());
            var resultEncodeSpan = Z85.Encode(data, outputEncode, out var encodeBytesConsumed, out var encodeCharsWritten);
            var resultDecodeRegular = Z85Extended.Decode(resultEncodeRegular);
            var resultDecodeSpan = Z85.Decode(outputEncode.Slice(0, encodeCharsWritten), outputDecode, out var decodeCharsConsumed, out var decodeBytesWritten);

            // assert
            // encode
            resultEncodeSpan.Should().Be(OperationStatus.Done);
            encodeBytesConsumed.Should().Be(data.Length);
            encodeCharsWritten.Should().Be(resultEncodeRegular.Length);
            outputEncode.Slice(0, encodeCharsWritten).ToString().Should().Be(resultEncodeRegular);

            // decode
            resultDecodeSpan.Should().Be(OperationStatus.Done);
            decodeCharsConsumed.Should().Be(resultEncodeRegular.Length);
            decodeBytesWritten.Should().Be(resultDecodeRegular.Length);
            outputDecode.Slice(0, decodeBytesWritten).ToArray().Should().BeEquivalentTo(resultDecodeRegular).And.BeEquivalentTo(data.ToArray());
        }
    }
}