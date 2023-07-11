using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Zord.EntityFrameworkCore;

public abstract class UnitOfWorkBase : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    protected UnitOfWorkBase(DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepositoryBase<T> Repository<T>(bool useCustomRepository = false)
        where T : class
    {
        _repositories.TryGetValue(typeof(T), out var value);
        if (value != null)
        {
            return (IRepositoryBase<T>)value;
        }

        if (useCustomRepository)
        {
            // use custom repository if available
            var customRepository = _context.GetService<IRepository<T>>();
            if (customRepository != null)
            {
                _repositories[typeof(T)] = customRepository;
                return customRepository;
            }
        }

        // use default repository
        var repository = new Repository<T>(_context);
        _repositories[typeof(T)] = repository;
        return repository;
    }

    public virtual int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
