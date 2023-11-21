using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Zord.Extensions
{
    public static class JsonConverter
    {
        /// <summary>
        /// Convert an instance of T to string (json as UTF8)
        /// </summary>
        public static string ToJsonString<T>(this T obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(obj, options);

            var bytes = Encoding.UTF8.GetBytes(json);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Read string data as an instance of T
        /// </summary>
        [return: MaybeNull]
        public static T ReadJsonAs<T>(this string value)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var bytes = Convert.FromBase64String(value);

            var json = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}