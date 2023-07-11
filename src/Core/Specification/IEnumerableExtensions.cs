using System;
using System.Collections.Generic;
using System.Linq;
using Zord.Core.Specification;

namespace Zord.Core.Specification
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ISpecification<T> specification)
        {
            if (specification?.Selector != null)
            {
                Func<T, bool> func = specification.Selector.Compile();
                return source.Where(func);
            }

            return source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, ISpecification<T> specification)
        {
            if (specification?.Selector != null && condition)
            {
                Func<T, bool> func = specification.Selector.Compile();
                return source.Where(func);
            }

            return source;
        }
    }
}