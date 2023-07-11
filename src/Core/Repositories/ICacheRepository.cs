using System.Threading;
using System.Threading.Tasks;

namespace Zord.Core.Repositories
{
    /// <summary>
    ///     Can be used to auto load list of T on cache,
    ///         query, add, update, remove instances of T.
    /// </summary>
    public interface ICacheRepository<T> : IRepository<T>
        where T : class
    {
        /// <summary>
        ///     delete list of T on cache.
        /// </summary>
        Task RemoveCacheAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     reload list of T on cache.
        /// </summary>
        Task ReloadCacheAsync(CancellationToken cancellationToken = default);
    }
}