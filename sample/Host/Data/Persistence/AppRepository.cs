using Zord.Core.Entities.Interfaces;
using Zord.EntityFrameworkCore;

namespace Host.Data.Persistence
{
    public class AppRepository<TEntity> : RepositoryBase<TEntity>, IAppRepository<TEntity>
        where TEntity : class, IEntity
    {
        public AppRepository(AlphaDbContext context) : base(context)
        {
        }
    }
}
