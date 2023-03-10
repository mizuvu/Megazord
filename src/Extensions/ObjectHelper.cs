using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Zord.Extensions
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Get [DisplayName] attribute of property
        /// </summary>
        public static string? GetDisplayName(this Type type)
        {
            var displayName = type
              .GetCustomAttributes(typeof(DisplayNameAttribute), true)
              .FirstOrDefault() as DisplayNameAttribute;

            return displayName?.DisplayName;
        }

        /// <summary>
        /// Get [Description] attribute of property
        /// </summary>
        public static string? GetDescription(this Type type)
        {
            var displayName = type
              .GetCustomAttributes(typeof(DescriptionAttribute), true)
              .FirstOrDefault() as DescriptionAttribute;

            return displayName?.Description;
        }

        /// <summary>
        /// Get [Display(Name={value})] attribute of property
        /// </summary>
        public static string? GetNameOfDisplay(this FieldInfo type)
        {
            DisplayAttribute? displayAttr = type
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Name;
        }

        /// <summary>
        /// Get [Display(Name={Description})] attribute of property
        /// </summary>
        public static string? GetDescriptionOfDisplay(this FieldInfo type)
        {
            DisplayAttribute? displayAttr = type
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Description;
        }

        /// <summary>
        /// Get all names of property in class
        /// </summary>
        public static IEnumerable<string> GetPropertyNames(this Type typeOfObj)
        {
            // get props in class
            var fields = typeOfObj.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var list = new List<string>();

            foreach (FieldInfo fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue != null)
                {
                    list.Add(propertyValue.ToString() ?? string.Empty);
                }
                //TODO - take descriptions from description attribute
            }

            return list;
        }

        /// <summary>
        /// Get Name of Property in a Object
        /// </summary>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            var memberExpression = expr.Body as MemberExpression;
            if (memberExpression is null)
                throw new ArgumentException($"The provided expression contains a {expr.GetType().Name} which is not supported. Only simple member accessors (fields, properties) of an object are supported.");
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Get Name of Property in a Object
        /// </summary>
        public static string GetPropertyName<T>(Expression<Func<T, string>> expr)
        {
            return GetPropertyName<T, string>(expr);
        }
    }
}
