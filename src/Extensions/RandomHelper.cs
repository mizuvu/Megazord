using System;
using System.Linq;

namespace Zord.Extensions
{
    public static class RandomHelper
    {
        /// <summary>
        /// Generate a random string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string String(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}

