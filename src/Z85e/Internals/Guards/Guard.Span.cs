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
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> has less than <paramref name="minLength"/> items.
        /// </exception>
        public static void MustBeSizedAtLeast<T>(ReadOnlySpan<T> source, int minLength, string parameterName)
        {
            if (source.Length < minLength)
                throw new ArgumentException($"Span {parameterName} must be at least of length {minLength}!", parameterName);
        }

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

        /// <summary>
        /// Verifies, that the <paramref name="source"/> has a length between or equal to of <paramref name="minLength"/> and <paramref name="maxLength"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the spans</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> has less than <paramref name="minLength"/>, or more than <paramref name="maxLength"/> items.
        /// </exception>
        public static void MustBeSizedBetweenOrEqualTo<T>(ReadOnlySpan<T> source, int minLength, int maxLength, string parameterName)
        {
            MustBeSizedAtLeast(source, minLength, parameterName);
            MustBeSizedAtMost(source, maxLength, parameterName);
        }
    }
}
