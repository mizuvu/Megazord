using System;

namespace Zord.Extensions
{
    public static class StringHelper
    {
        /// <summary>
        /// Get characters from left to character input
        /// </summary>
        public static string LeftToChar(this string value, string c)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // get poisition of char in value
            int idx = value.IndexOf(c);

            if (idx < 0) // < 0 is not contain char in value
                return value;

            return value[..idx]; //value.Substring(0, idx);
        }

        /// <summary>
        /// Get number of characters from left
        /// </summary>
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return value.Length <= maxLength
                   ? value
                   : value[..maxLength];
        }
    }
}
