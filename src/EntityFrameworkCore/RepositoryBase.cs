namespace Zord.EntityFrameworkCore;

public abstract class RepositoryBase<TEntity> : QueryRepository<TEntity>, IRepositoryBase<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(DbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual void Add(TEntity entity) => _dbSet.Add(entity);

    public virtual void AddRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

    public virtual void Update(TEntity entity) => _dbSet.Update(entity);

    public virtual void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    public virtual void Remove(TEntity entity) => _dbSet.Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    public virtual int SaveChanges() => _context.SaveChanges();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, bool tracking = false)
    {
        return tracking
            ? await _dbSet.ToListAsync(cancellationToken)
            : await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByKeyAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
    {
        return await _context.FindAsync<TEntity>(id, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByKeyAsync(object?[] key, CancellationToken cancellationToken = default)
    {
        return await _context.FindAsync<TEntity>(key, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}