﻿namespace Zord.EntityFrameworkCore;

public class Repository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity>
    where TEntity : class
{
    public Repository(DbContext context) : base(context)
    {
    }
}

public class Repository<TEntity, TContext> : Repository<TEntity>, IRepository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    public Repository(TContext context) : base(context)
    {
    }
}