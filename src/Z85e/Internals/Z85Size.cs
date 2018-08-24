namespace CoenM.Encoding.Internals
{
    using System;
    using System.Runtime.CompilerServices;

    internal static class Z85Size
    {
        /// <summary>
        /// </summary>
        /// <param name="byteLength"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CalculateEncodedSize(int byteLength)
        {
            //  Accepts only byte arrays bounded to 4 bytes
            if (byteLength % 4 != 0)
                throw new ArgumentOutOfRangeException("Data length should be multiple of 4.");

            return byteLength * 5 / 4;
        }


        /// <summary>
        /// </summary>
        /// <param name="stringLength"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CalculateDecodedSize(int stringLength)
        {
            //  Accepts only strings bounded to 5 bytes
            if (stringLength % 5 != 0)
                throw new ArgumentOutOfRangeException("Length of encoded string should be multiple of 5.");

            return stringLength * 4 / 5;
        }
    }
}