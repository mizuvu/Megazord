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
            if (specification?.Expression != null)
            {
                Func<T, bool> func = specification.Expression.Compile();
                return source.Where(func);
            }

            return source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, ISpecification<T> specification)
        {
            if (specification?.Expression != null && condition)
            {
                Func<T, bool> func = specification.Expression.Compile();
                return source.Where(func);
            }

            return source;
        }
    }
}