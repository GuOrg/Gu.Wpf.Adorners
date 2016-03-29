namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;

    internal static class Ensure
    {
        internal static void NotSame<T>(T x, T y, string xParameterName, string yParametername)
        {
            Debug.Assert(!string.IsNullOrEmpty(xParameterName), $"{nameof(xParameterName)} cannot be null");
            Debug.Assert(!string.IsNullOrEmpty(yParametername), $"{nameof(yParametername)} cannot be null");
            if (ReferenceEquals(x, y))
            {
                var message = $"Expected {xParameterName} and {yParametername} to not be the same instance.";
                throw new ArgumentException(message, $"{xParameterName}, {yParametername}");
            }
        }

        internal static void NotNull<T>(T value, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        internal static void IsTrue(bool condition, string parameterName, string message)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (!condition)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message, parameterName);
                }
                else
                {
                    throw new ArgumentException(parameterName);
                }
            }
        }

        internal static void Equal<T>(T value, T expected, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (!Equals(value, expected))
            {
                var message = $"Expected {parameterName} to be: {expected.ToStringOrNull()}, was: {value.ToStringOrNull()}";
                throw new ArgumentException(message, parameterName);
            }
        }

        internal static void NotEqual<T>(T value, T expected, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (Equals(value, expected))
            {
                var message = $"Expected {parameterName} to not be: {expected.ToStringOrNull()}";
                throw new ArgumentException(message, parameterName);
            }
        }

        private static string ToStringOrNull<T>(this T value)
        {
            if (value == null)
            {
                return "null";
            }

            return value.ToString();
        }
    }
}