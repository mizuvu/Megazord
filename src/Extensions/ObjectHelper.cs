using System;
using System.Collections;
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
        public static string? GetDisplayName(this MemberInfo memberInfo)
        {
            var displayName = memberInfo
              .GetCustomAttributes(typeof(DisplayNameAttribute), true)
              .FirstOrDefault() as DisplayNameAttribute;

            return displayName?.DisplayName;
        }

        /// <summary>
        /// Get [Description] attribute of property
        /// </summary>
        public static string? GetDescription(this MemberInfo memberInfo)
        {
            var displayName = memberInfo
              .GetCustomAttributes(typeof(DescriptionAttribute), true)
              .FirstOrDefault() as DescriptionAttribute;

            return displayName?.Description;
        }

        /// <summary>
        /// Get [Display(Name={value})] attribute of Type
        /// </summary>
        public static string? GetNameInDisplay(this MemberInfo memberInfo)
        {
            DisplayAttribute? displayAttr = memberInfo
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Name;
        }

        /// <summary>
        /// Get [Display(Name={Description})] attribute of Type
        /// </summary>
        public static string? GetDescriptionInDisplay(this MemberInfo memberInfo)
        {
            DisplayAttribute? displayAttr = memberInfo
                .GetCustomAttributes(typeof(DisplayAttribute), true)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Description;
        }

        /// <summary>
        /// Get Name of Property in an Object
        /// </summary>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            if (!(expr.Body is MemberExpression memberExpression))
                throw new ArgumentException($"The provided expression contains a {expr.GetType().Name} which is not supported. Only simple member accessors (fields, properties) of an object are supported.");
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Get all properties values of an object
        /// </summary>
        public static IDictionary<string, object> GetValues(this object obj)
        {
            var result = new Dictionary<string, object>();

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object? value = descriptor.GetValue(obj);

                if (value != null)
                {
                    result[name] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// check object type is a list
        /// </summary>
		public static bool IsList(this object obj)
        {
            return obj != null
                && obj is IList
                && obj.GetType().IsGenericType
                && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
