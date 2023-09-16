using Microsoft.EntityFrameworkCore;
using Zord.Specification;

namespace Zord.EntityFrameworkCore.Extensions
{
    public static class DbSetSpecificationExtensions
    {
        /// <summary>
        ///     query from DbSet filter by specification builder
        /// </summary>
        private static IQueryable<T> Where<T>(this DbSet<T> dbSet,
            ISpecification<T> specification,
            bool tracking = true)
            where T : class
        {
            var query = dbSet.AsQueryable();

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
        public static Task<List<T>> ToListAsync<T>(this DbSet<T> dbSet,
            ISpecification<T> specification,
            bool tracking = true,
            CancellationToken cancellationToken = default)
            where T : class
            => dbSet.Where(specification, tracking).ToListAsync(cancellationToken);

        /// <summary>
        ///     Get single instance of T by specification
        /// </summary>
        public static Task<T> SingleAsync<T>(this DbSet<T> dbSet,
            ISpecification<T> specification,
            bool tracking = true,
            CancellationToken cancellationToken = default)
            where T : class
            => dbSet.Where(specification, tracking).SingleAsync(cancellationToken);

        /// <summary>
        ///     Get single instance of T or default by specification
        /// </summary>
        public static Task<T?> SingleOrDefaultAsync<T>(this DbSet<T> dbSet,
            ISpecification<T> specification,
            bool tracking = true,
            CancellationToken cancellationToken = default)
            where T : class
            => dbSet.Where(specification, tracking).SingleOrDefaultAsync(cancellationToken);

        /// <summary>
        ///     Get first instance of T by specification
        /// </summary>
        public static Task<T> FirstAsync<T>(this DbSet<T> dbSet,
            ISpecification<T> specification,
            bool tracking = true,
            CancellationToken cancellationToken = default)
            where T : class
            => dbSet.Where(specification, tracking).FirstAsync(cancellationToken);

        /// <summary>
        ///     Get first instance of T or default by specification
        /// </summary>
        public static Task<T?> FirstOrDefaultAsync<T>(this DbSet<T> dbSet,
            ISpecification<T> specification,
            bool tracking = true,
            CancellationToken cancellationToken = default)
            where T : class
            => dbSet.Where(specification, tracking).FirstOrDefaultAsync(cancellationToken);
    }
}
