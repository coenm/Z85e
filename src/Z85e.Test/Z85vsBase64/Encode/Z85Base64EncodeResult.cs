namespace CoenM.Encoding.Test.Z85vsBase64.Encode
{
    using System;
    using System.Buffers;

    using JetBrains.Annotations;

    public class Z85Base64EncodeResult
    {
        private readonly OperationStatus status;
        private readonly int bytesConsumed;
        private readonly int bytesWritten;

        public Z85Base64EncodeResult(OperationStatus status, int charactersBlocksConsumed, bool allCharsConsumed, int bytesConsumed, int byteBlocksWritten, int bytesWritten)
        {
            this.status = status;
            this.bytesConsumed = bytesConsumed;
            this.bytesWritten = bytesWritten;
            CharactersBlocksConsumed = charactersBlocksConsumed;
            AllCharsConsumed = allCharsConsumed;
            ByteBlocksWritten = byteBlocksWritten;
        }

        [PublicAPI] public string Status => status.ToString();

        [PublicAPI] public int CharactersBlocksConsumed { get; }

        [PublicAPI] public bool AllCharsConsumed { get; }

        [PublicAPI] public int ByteBlocksWritten { get; }

        public override string ToString()
        {
            return $"{nameof(Status)}: {Status}{Environment.NewLine}{nameof(CharactersBlocksConsumed)}: {CharactersBlocksConsumed}{Environment.NewLine}{nameof(AllCharsConsumed)}: {AllCharsConsumed}{Environment.NewLine}{nameof(ByteBlocksWritten)}: {ByteBlocksWritten}{Environment.NewLine}{nameof(bytesConsumed)}:{bytesConsumed}{Environment.NewLine}{nameof(bytesWritten)}:{bytesWritten}";
        }
    }
}
