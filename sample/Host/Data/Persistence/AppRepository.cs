using Zord.Entities.Interfaces;

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
