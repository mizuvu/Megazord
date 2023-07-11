using System;
using System.Linq;
using System.Linq.Expressions;
using Zord.Core.Specification;

namespace Zord.Core.Specification
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// only query data when condition is true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> whereClause)
        {
            if (condition)
            {
                return query.Where(whereClause);
            }

            return query;
        }

        /// <summary>
        /// query data by specification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecification<T> specification)
        {
            if (specification?.Selector != null)
            {
                return query.Where(specification.Selector);
            }

            return query;
        }

        /// <summary>
        /// only query data by specification when condition is true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, ISpecification<T> specification)
        {
            if (specification?.Selector != null && condition)
            {
                return query.Where(specification.Selector);
            }

            return query.AsQueryable();
        }
    }
}