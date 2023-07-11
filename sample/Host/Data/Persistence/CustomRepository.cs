using Zord.Core.Entities.Interfaces;
using Zord.EntityFrameworkCore;

namespace Host.Data.Persistence;

public class CustomRepository<TEntity> : RepositoryBase<TEntity>, IAppRepository<TEntity>
    where TEntity : class, IEntity
{
    public CustomRepository(AlphaDbContext context) : base(context)
    {
    }

    public override Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, bool tracking = false)
    {
        Console.WriteLine("Use from Custom repository");
        return base.GetAllAsync(cancellationToken, tracking);
    }
}
