namespace CoenM.Encoding.Internals
{
    using System;
    using System.Runtime.CompilerServices;

    internal static class Z85Size
    {
        /// <summary>Calculate encoded size.</summary>
        /// <param name="byteLength">Length of the bytes.</param>
        /// <returns>Size needed for encoding.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="byteLength"/> is not a multiple of 4.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CalculateEncodedSize(int byteLength)
        {
            // Accepts only byte arrays bounded to 4 bytes
            if (byteLength % 4 != 0)
                throw new ArgumentOutOfRangeException($"Data length should be multiple of 4.");

            return byteLength * 5 / 4;
        }

        /// <summary>Calculate decoded size.</summary>
        /// <param name="stringLength">Length of the string to decode.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="stringLength"/> is not a multiple of 5.</exception>
        /// <returns>Size needed for decoding.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CalculateDecodedSize(int stringLength)
        {
            // Accepts only strings bounded to 5 bytes
            if (stringLength % 5 != 0)
                throw new ArgumentOutOfRangeException($"Length of encoded string should be multiple of 5.");

            return stringLength * 4 / 5;
        }
    }
}
