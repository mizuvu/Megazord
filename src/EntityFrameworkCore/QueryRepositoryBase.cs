using System.Linq.Expressions;
using Zord.Core.Specification;

namespace Zord.EntityFrameworkCore;

public abstract class QueryRepository<TEntity> : IQueryRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;

    protected QueryRepository(DbContext context) => _context = context;

    public virtual IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
    {
        return _context.Set<TEntity>().Include(navigationPropertyPath);
    }

    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool noTracking = false)
    {
        var query = _context.Set<TEntity>().Where(expression);

        return noTracking
            ? query.AsNoTracking()
            : query;
    }

    public virtual IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> expression, bool noTracking = false)
    {
        var query = _context.Set<TEntity>().WhereIf(condition, expression);

        return noTracking
            ? query.AsNoTracking()
            : query;
    }

    public virtual IQueryable<TEntity> Where(ISpecification<TEntity> specification, bool noTracking = false)
    {
        var query = _context.Set<TEntity>().Where(specification);

        return noTracking
            ? query.AsNoTracking()
            : query;
    }

    public virtual IQueryable<TEntity> WhereIf(bool condition, ISpecification<TEntity> specification, bool noTracking = false)
    {
        var query = _context.Set<TEntity>().WhereIf(condition, specification);

        return noTracking
            ? query.AsNoTracking()
            : query;
    }
}