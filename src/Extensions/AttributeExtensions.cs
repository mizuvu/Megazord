using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zord.Extensions
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Get value from [DisplayName] attribute
        /// </summary>
        public static string? GetDisplayName(this MemberInfo memberInfo)
        {
            var displayName = memberInfo
              .GetCustomAttributes(typeof(DisplayNameAttribute), true)[0] as DisplayNameAttribute;

            return displayName?.DisplayName;
        }

        /// <summary>
        /// Get value from [Description] attribute
        /// </summary>
        public static string? GetDescription(this MemberInfo memberInfo)
        {
            var displayName = memberInfo
              .GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;

            return displayName?.Description;
        }

        /// <summary>
        /// Get value of Name from [Display(Name={value})] attribute
        /// </summary>
        public static string? GetNameOfDisplay(this MemberInfo memberInfo)
        {
            DisplayAttribute? displayAttr = memberInfo
                .GetCustomAttributes(typeof(DisplayAttribute), true)[0] as DisplayAttribute;

            return displayAttr?.Name;
        }

        /// <summary>
        /// Get value of Description from [Display(Name={Description})] attribute
        /// </summary>
        public static string? GetDescriptionOfDisplay(this MemberInfo memberInfo)
        {
            DisplayAttribute? displayAttr = memberInfo
                .GetCustomAttributes(typeof(DisplayAttribute), true)[0] as DisplayAttribute;

            return displayAttr?.Description;
        }
    }
}
