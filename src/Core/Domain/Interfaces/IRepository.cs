using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zord.Core.Domain.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T>
        where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}