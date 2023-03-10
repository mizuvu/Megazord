using System.Data;
using System.Data.Common;

namespace Zord.EntityFrameworkCore.Dapper
{
    public interface IDapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string query, Func<DbDataReader, T> map);
        Task<IEnumerable<T>> QueryAsync<T>(string query, object? param, IDbTransaction? transaction);
        Task<IEnumerable<T>> QueryAsync<T>(string query, CancellationToken cancellationToken = default);
    }

    public interface IDapper<TContext> : IDapper
        where TContext : IDbFactory
    {
    }
}