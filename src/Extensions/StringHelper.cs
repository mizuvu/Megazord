﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Zord.Extensions
{
    public static class StringHelper
    {
        /// <summary>
        ///     Get characters from left to character input
        /// </summary>
        public static string Left(this string value, string c)
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
        ///     Get substring of specified number of characters on the left.
        /// </summary>
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            length = Math.Abs(length);

            return value.Length <= length
                   ? value
                   : value[..length];
        }

        /// <summary>
        ///     Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            length = Math.Abs(length);

            return value.Length <= length
                   ? value
                   : value[(value.Length - length)..];
        }

        /// <summary>
        ///     Get substring of specified number of characters on the right.
        /// </summary>
        public static void SetIfNullOrEmpty(this string? value, string newValue)
        {
            if (string.IsNullOrEmpty(value))
                value = newValue;
        }
    }
}
