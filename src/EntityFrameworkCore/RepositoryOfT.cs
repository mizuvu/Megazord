using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zord.Core.Domain.Interfaces;
using Zord.Core.Specifications;

namespace Zord.EntityFrameworkCore
{
    public abstract class RepositoryOfT<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _context;

        protected RepositoryOfT(DbContext context) => _context = context;

        public virtual IQueryable<T> AsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> AsNoTracking()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public virtual IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public virtual IQueryable<T> Where(ISpecification<T> specification)
        {
            return _context.Set<T>().Where(specification);
        }

        public virtual IQueryable<T> WhereIf(bool condition, ISpecification<T> specification)
        {
            return _context.Set<T>().WhereIf(condition, specification);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}