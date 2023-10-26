using System.Text.RegularExpressions;

namespace Zord.Extensions
{
    public static class RegexExtensions
    {
        /// <summary>
        ///     Remove Emoji icon from string
        /// </summary>
        public static string RemoveEmoji(this string text)
        {
            return Regex.Replace(text, @"\p{Cs}", "");
        }
    }
}
