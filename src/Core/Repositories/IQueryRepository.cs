using System;
using System.Linq;
using System.Linq.Expressions;
using Zord.Core.Specification;

namespace Zord.Core.Repositories
{
    /// <summary>
    ///     Use to query instances of T.
    /// </summary>
    public interface IQueryRepository<T>
        where T : class
    {
        /// <summary>
        ///     Query instances of T with Include.
        /// </summary>
        IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);

        /// <summary>
        ///     Query instances of T by expression,
        ///         return query AsNoTracking if noTracking is true.
        /// </summary>
        IQueryable<T> Where(Expression<Func<T, bool>> expression, bool noTracking = false);

        /// <summary>
        ///     Query instances of T by expression if condition is true,
        ///         return query AsNoTracking if noTracking is true.
        /// </summary>
        IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> expression, bool noTracking = false);

        /// <summary>
        ///     Query instances of T by specification,
        ///         return query AsNoTracking if noTracking is true.
        /// </summary>
        IQueryable<T> Where(ISpecification<T> specification, bool noTracking = false);

        /// <summary>
        ///     Query instances of T by specification if condition is true,
        ///         return query AsNoTracking if noTracking is true.
        /// </summary>
        IQueryable<T> WhereIf(bool condition, ISpecification<T> specification, bool noTracking = false);
    }
}