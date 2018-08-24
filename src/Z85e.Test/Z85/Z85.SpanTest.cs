namespace CoenM.Encoding.Test.Z85
{
    using System;
    using System.Buffers;

    using CoenM.Encoding.Test.TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85SpanTest
    {
        private readonly byte[] _helloWorldBytes = { 0x86, 0x4F, 0xD2, 0x6F, 0xB5, 0x59, 0xF7, 0x5B };
        private const string HelloWorldString = "HelloWorld";

        [Fact]
        public void CalculateDecodedSizeTest()
        {
            var result = Sut.CalculateDecodedSize(HelloWorldString.AsSpan());
            Assert.Equal(_helloWorldBytes.Length, result);
        }

        [Fact]
        public void CalculateDecodedSizeThrowsExceptionWhenInputHasWrongSizeTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Sut.CalculateDecodedSize(null));
        }

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
        [ClassData(typeof(Z85Samples))]
        public void DecodeTest(byte[] data, string encoded)
        {
            // arrange
            var source = encoded.AsSpan();
            var destination = new byte[data.Length + 10].AsSpan();

            // act
            var result = Sut.Decode(source, destination, out var charsConsumed, out var bytesWritten);

            // assert
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(encoded.Length, charsConsumed);
            Assert.Equal(data.Length, bytesWritten);
            Assert.Equal(destination.Slice(0, bytesWritten).ToArray(), data);
        }

        [Theory]
        [ClassData(typeof(Z85Samples))]
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
        public void DecodeNullReturnsNullTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Null(Sut.Decode(null));
        }

        [Fact]
        public void EncodeNullReturnsNullTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Null(Sut.Encode(null));
        }

        [Theory]
        [ClassData(typeof(Z85InvalidEncodedLengths))]
        public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Decode(new string('a', size)));
        }

        [Fact]
        public void MultipleEncodedStringsDecodeToSameBytes()
        {
            // arrange
            const string encoded1 = "00000";
            const string encoded2 = "%nSc1";

            // act
            var result1 = Sut.Decode(encoded1);
            var result2 = Sut.Decode(encoded2);

            // assert
            Assert.Equal(result1, result2);
        }
    }
}