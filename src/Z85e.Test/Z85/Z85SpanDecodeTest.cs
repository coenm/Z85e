namespace CoenM.Encoding.Test.Z85
{
    using System;
    using System.Buffers;

    using CoenM.Encoding.Test.TestData;

    using Xunit;

    using Sut = Encoding.Z85;

    public class Z85SpanDecodeTest
    {
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


        [Fact]
        public void DecodeNullReturnsNullTest()
        {
            // arrange
            Span<char> source = null;
            Span<byte> destination = new byte[0];

            // act

            // assert

            // ReSharper disable once AssignNullToNotNullAttribute
            var result = Sut.Decode(source, destination, out var _, out var __);

        }

        [Theory]
        [ClassData(typeof(Z85InvalidEncodedStrings))]
        public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(string encoded)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Decode(encoded));
        }







        // [Fact]
        // public void CalculateDecodedSizeTest()
        // {
        //     var result = Sut.CalculateDecodedSize(HelloWorldString.AsSpan());
        //     Assert.Equal(_helloWorldBytes.Length, result);
        // }

        [Fact]
        public void CalculateDecodedSizeThrowsExceptionWhenInputHasWrongSizeTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Sut.CalculateDecodedSize(null));
        }





        // [Theory]
        // [ClassData(typeof(Z85InvalidEncodedLengths))]
        // public void DecodeThrowsExceptionWhenInputHasWrongSizeTest(int size)
        // {
        //     Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Decode(new string('a', size)));
        // }
    }
}