using System.Buffers;
using JetBrains.Annotations;

namespace CoenM.Encoding.Test.Z85vsBase64.Decode
{
    public class Z85Base64DecodeResult
    {
        private readonly OperationStatus _status;

        public Z85Base64DecodeResult(OperationStatus status, int charactersBlocksConsumed, bool allCharsConsumed, int byteBlocksWritten)
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
    }
}