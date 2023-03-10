using System;
using System.Linq;
using System.Linq.Expressions;
using Zord.Core.Specifications;

namespace Zord.Core.Domain.Interfaces
{
    public interface IRepositoryBase<T>
        where T : class
    {
        IQueryable<T> AsQueryable();

        IQueryable<T> AsNoTracking();

        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> expression);

        IQueryable<T> Where(ISpecification<T> specification);

        IQueryable<T> WhereIf(bool condition, ISpecification<T> specification);
    }
}