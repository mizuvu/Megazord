using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zord.Core.Repositories
{
    /// <summary>
    ///     Can be used to query and update instances of T.
    /// </summary>
    public interface IRepositoryBase<T> : IQueryRepository<T>, ICommandRepository<T>
        where T : class
    {
        /// <summary>
        ///     Asynchronous query all instances of T.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default, bool tracking = false);

        /// <summary>
        ///     Asynchronous find a instance of T by key.
        /// </summary>
        Task<T?> GetByKeyAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Asynchronous find a instance of T by object ids.
        /// </summary>
        Task<T?> GetByKeyAsync(object?[] id, CancellationToken cancellationToken = default);
    }
}