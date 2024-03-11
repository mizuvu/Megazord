using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Zord.Extensions
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        /// <summary>
        /// Convert an instance of T to Json as Base64 (UTF8)
        /// </summary>
        public static string ToBase64Json<T>(this T obj)
        {
            var json = JsonSerializer.Serialize(obj, options);

            var bytes = Encoding.UTF8.GetBytes(json);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Read Base64 (UTF8) data as an instance of T
        /// </summary>
        [return: MaybeNull]
        public static T ReadBase64JsonAs<T>(this string value)
        {
            var bytes = Convert.FromBase64String(value);

            var json = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<T>(json, options);
        }

        /// <summary>
        /// Convert an instance of T to Json
        /// </summary>
        public static string ToJson<T>(this T obj) => JsonSerializer.Serialize(obj, options);

        /// <summary>
        /// Read json as an instance of T
        /// </summary>
        [return: MaybeNull]
        public static T ReadJsonAs<T>(this string json) => JsonSerializer.Deserialize<T>(json, options);
    }
}