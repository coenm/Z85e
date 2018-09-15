using System;
using System.Buffers;
using JetBrains.Annotations;

namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    public class Z85Base64EncodeResult
    {
        private readonly OperationStatus _status;

        public Z85Base64EncodeResult(OperationStatus status, int charactersBlocksConsumed, bool allCharsConsumed, int byteBlocksWritten)
        {
            _status = status;
            CharactersBlocksConsumed = charactersBlocksConsumed;
            AllCharsConsumed = allCharsConsumed;
            ByteBlocksWritten = byteBlocksWritten;
        }

        [PublicAPI] public string Status => _status.ToString();

        [PublicAPI] public int CharactersBlocksConsumed { get; }

        [PublicAPI] public bool AllCharsConsumed { get; }

        [PublicAPI] public int ByteBlocksWritten { get; }

        public override string ToString()
        {
            return $"{nameof(Status)}: {Status}{Environment.NewLine}{nameof(CharactersBlocksConsumed)}: {CharactersBlocksConsumed}{Environment.NewLine}{nameof(AllCharsConsumed)}: {AllCharsConsumed}{Environment.NewLine}{nameof(ByteBlocksWritten)}: {ByteBlocksWritten}";
        }
    }
}