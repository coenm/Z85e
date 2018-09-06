// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable All

namespace CoenM.Encoding.Test.Z85
{
    using System;
    using System.Buffers;

    using CoenM.Encoding.Test.TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85SpanEncodeTest
    {
        private readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
        private const string HelloWorldString = "HelloWorld";

        [Fact]
        public void CalculateEncodedSizeTest()
        {
            var result = Sut.CalculateEncodedSize(_helloWorldBytes.AsSpan());
            Assert.Equal(HelloWorldString.Length, result);
        }

        [Fact]
        public void CalculateEncodedSizeThrowsExceptionWhenInputHasWrongSizeTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Sut.CalculateEncodedSize(null));
        }

        [Theory]
        [ClassData(typeof(Z85InvalidDataLengths))]
        public void CalculateEncodedSizeThrowsExceptionWhenLengthIsWrongTest(int length)
        {
            var source = new byte[length].AsMemory();

            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.CalculateEncodedSize(source.Span));
        }

        [Theory]
        [ClassData(typeof(ModeAndFinalBlockOptions))]
        public void EncodeEmptyTest(Z85Mode mode, bool isFinalBlock)
        {
            // arrange
            Span<byte> source = Span<byte>.Empty;
            Span<char> destination = new char[100];

            // act
            var result = Sut.Encode(source, destination, out var bytesConsumed, out var charsWritten, mode, isFinalBlock);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(0, bytesConsumed);
            Assert.Equal(0, charsWritten);
        }


        [Theory]
        [ClassData(typeof(NeedMoreDataEncodeScenario))]
        public void BasicEncodingWithFinalBlockFalseAndPaddingModeWithKnownInputNeedMoreDataTest(EncodeData scenario)
        {
            // arrange
            ReadOnlySpan<byte> source = scenario.Data.Span;
            Span<char> destination = new char[scenario.ExpectedCharsWritten + 20];

            // act
            var result = Sut.Encode(source, destination, out var bytesConsumed, out var charsWritten, Z85Mode.Padding, isFinalBlock: false);

            // assert
            scenario.Assert(result, bytesConsumed, charsWritten, destination);
        }


        [Theory]
        [ClassData(typeof(StrictZ85Samples))]
        public void EncodeTest(byte[] data, string encoded)
        {
            // arrange
            var source = data.AsSpan();
            var destination = new char[encoded.Length+10].AsSpan();

            // act
            var result = Sut.Encode(source, destination, out var bytesConsumed, out var charsWritten);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(encoded.Length, charsWritten);
            Assert.Equal(data.Length, bytesConsumed);
            Assert.Equal(destination.Slice(0, charsWritten).ToString(), encoded);
        }

        [Theory]
        [ClassData(typeof(Z85InvalidDataLengths))]
        public void EncodeThrowsExceptionWhenInputHasWrongSizeTest(int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Encode(new byte[length]));
        }

        [Fact]
        public void EncodeNullReturnsNullTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Null(Sut.Encode(null));
        }
    }
}