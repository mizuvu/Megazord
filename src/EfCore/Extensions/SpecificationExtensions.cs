using Zord.EntityFrameworkCore.Extensions;
using Zord.Specification;

namespace Zord.EntityFrameworkCore.Extensions
{
    public static class SpecificationExtensions
    {
        /// <summary>
        ///     build a queryable filter by specification & tracking behaviour
        /// </summary>
        public static IQueryable<T> Where<T>(this DbContext context,
            ISpecification<T> specification,
            bool tracking = true)
            where T : class
        {
            var query = context.Set<T>().AsQueryable();

            // filter by specification
            if (specification?.Expression != null)
            {
                query = query.Where(specification.Expression);
            }

            // check Tracking
            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        /// <summary>
        ///     Get list instances of T by specification
        /// </summary>
        public static Task<List<T>> ToListAsync<T>(this DbContext context,
            ISpecification<T> specification,
            CancellationToken cancellationToken = default,
            bool tracking = true)
            where T : class
            => context.Where(specification, tracking).ToListAsync(cancellationToken);

        /// <summary>
        ///     Get single instance of T by specification
        /// </summary>
        public static Task<T> SingleAsync<T>(this DbContext context,
            ISpecification<T> specification,
            CancellationToken cancellationToken = default,
            bool tracking = true)
            where T : class
            => context.Where(specification, tracking).SingleAsync(cancellationToken);

        /// <summary>
        ///     Get single instance of T or default by specification
        /// </summary>
        public static Task<T?> SingleOrDefaultAsync<T>(this DbContext context,
            ISpecification<T> specification,
            CancellationToken cancellationToken = default,
            bool tracking = true)
            where T : class
            => context.Where(specification, tracking).SingleOrDefaultAsync(cancellationToken);

        /// <summary>
        ///     Get first instance of T by specification
        /// </summary>
        public static Task<T> FirstAsync<T>(this DbContext context,
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
            where T : class
            => context.Where(specification, true).FirstAsync(cancellationToken);

        public static Task<T> FirstAsync<T>(this DbContext context,
            ISpecification<T> specification,
            bool tracking,
            CancellationToken cancellationToken = default)
            where T : class
            => context.Where(specification, tracking).FirstAsync(cancellationToken);

        /// <summary>
        ///     Get first instance of T or default by specification
        /// </summary>
        public static Task<T?> FirstOrDefaultAsync<T>(this DbContext context,
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
            where T : class
            => context.Where(specification, true).FirstOrDefaultAsync(cancellationToken);

        public static Task<T?> FirstOrDefaultAsync<T>(this DbContext context,
            ISpecification<T> specification,
            bool tracking,
            CancellationToken cancellationToken = default)
            where T : class
            => context.Where(specification, tracking).FirstOrDefaultAsync(cancellationToken);
    }
}
