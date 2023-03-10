using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Zord.EntityFrameworkCore.Dapper;

public abstract class Dapper : IDapper
{
    private readonly DbContext _context;

    public Dapper(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, Func<DbDataReader, T> map)
    {
        // await RawSqlQuery(query, x => new T { Prop0 = (string)x[0], Prop1 = (string)x[1] });
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;

        await _context.Database.OpenConnectionAsync();

        using var result = await command.ExecuteReaderAsync();
        var entities = new List<T>();

        while (result.Read())
        {
            entities.Add(map(result));
        }

        return entities;
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, object? param, IDbTransaction? transaction)
    {
        return await _context.Database.GetDbConnection().QueryAsync<T>(query, param, transaction);
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, CancellationToken cancellationToken = default)
    {
        return await _context.Database.GetDbConnection().QueryAsync<T>(query, cancellationToken);
    }
}
