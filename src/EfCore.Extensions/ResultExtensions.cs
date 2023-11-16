﻿using Zord.Result;

namespace Zord.EntityFrameworkCore.Extensions;

public static class ResultExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;
        var count = await queryable.CountAsync(cancellationToken);
        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<T>(items, pageNumber, pageSize, count);
    }

    public static async Task<Result<IEnumerable<T>>> ToListResultAsync<T>(this IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        var items = await queryable.ToListAsync(cancellationToken);
        return Result<IEnumerable<T>>.Success(items);
    }

    public static async Task<Result<T>> FirstResultAsync<T>(this IQueryable<T> queryable,
        string objectName, object queryValue,
        CancellationToken cancellationToken = default)
    {
        var entry = await queryable.FirstOrDefaultAsync(cancellationToken);

        if (entry == null)
            return Result<T>.NotFound(objectName, queryValue);

        return Result<T>.Success(entry);
    }

    public static async Task<Result<T>> LastResultAsync<T>(this IQueryable<T> queryable,
        string objectName, object queryValue,
        CancellationToken cancellationToken = default)
    {
        var entry = await queryable.LastOrDefaultAsync(cancellationToken);

        if (entry == null)
            return Result<T>.NotFound(objectName, queryValue);

        return Result<T>.Success(entry);
    }

    public static async Task<Result<T>> SingleResultAsync<T>(this IQueryable<T> queryable,
        string objectName, object queryValue,
        CancellationToken cancellationToken = default)
    {
        var entry = await queryable.SingleOrDefaultAsync(cancellationToken);

        if (entry == null)
            return Result<T>.NotFound(objectName, queryValue);

        return Result<T>.Success(entry);
    }
}