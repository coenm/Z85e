using System;

namespace CoenM.Encoding.Internals.Guards
{
    /// <summary>
    /// Provides methods to protect against invalid parameters.
    /// </summary>
    internal static partial class Guard
    {
        /// <summary>
        /// Verifies, that the `source` span has the length of 'minSpan', or longer.
        /// </summary>
        /// <typeparam name="T">The element type of the spans</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> has less than <paramref name="minLength"/> items.
        /// </exception>
        public static void MustBeSizedAtLeast<T>(ReadOnlySpan<T> source, int minLength, string parameterName)
        {
            if (source.Length < minLength)
                throw new ArgumentOutOfRangeException($"Span {parameterName} must be at least of length {minLength}!", parameterName);
        }

        /// <inheritdoc cref="MustBeSizedAtLeast{T}(ReadOnlySpan{T},int,string)"/>
        public static void MustBeSizedAtLeast<T>(Span<T> source, int minLength, string parameterName) =>
            MustBeSizedAtLeast((ReadOnlySpan<T>)source, minLength, parameterName);


        /// <summary>
        /// Verifies, that the <paramref name="source"/> has a maximum length of <paramref name="maxLength"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the spans</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> has more than <paramref name="maxLength"/> items.
        /// </exception>
        public static void MustBeSizedAtMost<T>(ReadOnlySpan<T> source, int maxLength, string parameterName)
        {
            if (source.Length > maxLength)
                throw new ArgumentException($"Span {parameterName} must be at most of length {maxLength}!", parameterName);
        }

        /// <inheritdoc cref="MustBeSizedAtMost{T}(ReadOnlySpan{T},int,string)"/>
        public static void MustBeSizedAtMost<T>(Span<T> source, int maxLength, string parameterName) =>
            MustBeSizedAtMost((ReadOnlySpan<T>)source, maxLength, parameterName);

        /// <summary>Verifies, that the <paramref name="source"/> has a length between or equal to of <paramref name="minLength"/> and <paramref name="maxLength"/>.</summary>
        /// <typeparam name="T">The element type of the spans</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <exception cref="ArgumentException"><paramref name="source"/> has less than <paramref name="minLength"/>, or more than <paramref name="maxLength"/> items.</exception>
        public static void MustBeSizedBetweenOrEqualTo<T>(ReadOnlySpan<T> source, int minLength, int maxLength, string parameterName)
        {
            MustBeSizedAtLeast(source, minLength, parameterName);
            MustBeSizedAtMost(source, maxLength, parameterName);
        }

        /// <inheritdoc cref="MustBeSizedBetweenOrEqualTo{T}(ReadOnlySpan{T},int,int,string)"/>
        public static void MustBeSizedBetweenOrEqualTo<T>(Span<T> source, int minLength, int maxLength, string parameterName) =>
            MustBeSizedBetweenOrEqualTo((ReadOnlySpan<T>)source, minLength, maxLength, parameterName);


        /// <summary>Verifies, that the <paramref name="source"/> has a length with a multiple of <paramref name="mod"/>.</summary>
        /// <typeparam name="T">The element type of the spans</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="mod">Expected multiple of the spans length. Should have a positive value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="errorMessage">optional error message, otherwise a default error message will be used.</param>
        /// <exception cref="ArgumentException">Thrown when length of <paramref name="source"/> is not a multiple of <paramref name="mod"/>.</exception>
        public static void MustHaveSizeMultipleOf<T>(ReadOnlySpan<T> source, int mod, string parameterName, string errorMessage = "")
        {
            if (mod < 0)
                throw new NotSupportedException($"PROGRAMMING EXCEPTION, {nameof(mod)} cannot be negative.");

            if (mod == 0)
                return;

            if (source.Length % mod != 0)
            {
                var msg = errorMessage;
                if (string.IsNullOrWhiteSpace(msg))
                    msg = $"Span {parameterName} length is not a multiple of {mod}!";

                throw new ArgumentException(msg, parameterName);
            }
        }

        /// <inheritdoc cref="MustHaveSizeMultipleOf{T}(ReadOnlySpan{T},int,string,string)"/>
        public static void MustHaveSizeMultipleOf<T>(Span<T> source, int mod, string parameterName, string errorMessage = "") =>
            MustHaveSizeMultipleOf((ReadOnlySpan<T>)source, mod, parameterName, errorMessage);
    }
}
