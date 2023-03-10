using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zord.Extensions
{
    public class EnumData
    {
        public int Value { get; internal set; }
        public string StringValue { get; internal set; } = default!;
        public string Description { get; internal set; } = default!;
    }

    public static class EnumHelper
    {
        /// <summary>
        /// Get description of a value of Enum
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            object[] attr = enumValue.GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attr.Length > 0)
                return ((DescriptionAttribute)attr[0]).Description;
            string result = enumValue.ToString();
            result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2");
            result = Regex.Replace(result, "([A-Za-z])([0-9])", "$1 $2");
            result = Regex.Replace(result, "([0-9])([A-Za-z])", "$1 $2");
            result = Regex.Replace(result, "(?<!^)(?<! )([A-Z][a-z])", " $1");
            return result;
        }

        /// <summary>
        /// Get all description of Enum
        /// </summary>
        public static List<string> GetDescriptionList(this Enum enumValue)
        {
            string result = enumValue.GetDescription();
            return result.Split(',').ToList();
        }

        /// <summary>
        /// Get enum values & descriptions
        /// </summary>
        public static IEnumerable<EnumData> GetOptions<T>()
            where T : Enum
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            List<EnumData> enumValList = new List<EnumData>();

            foreach (var e in Enum.GetValues(enumType))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                var data = new EnumData
                {
                    Value = (int)e,
                    StringValue = e.ToString(),
                    Description = attributes.Length > 0 ? attributes[0].Description : "",
                };

                enumValList.Add(data);
            }

            return enumValList;
        }
    }
}
