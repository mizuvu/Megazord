using Zord.Entities.Interfaces;
using Zord.Repository.EntityFrameworkCore;

namespace Sample.Data.Persistence;

public class CustomRepository<TEntity> : RepositoryBase<TEntity>, IAppRepository<TEntity>
    where TEntity : class, IEntity
{
    public CustomRepository(AlphaDbContext context) : base(context)
    {
    }

    public override Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Use from Custom repository");
        return base.ToListAsync(cancellationToken);
    }
}
