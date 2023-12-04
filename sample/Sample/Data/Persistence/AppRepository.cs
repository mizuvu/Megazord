using Sample.Data;
using Zord.Entities.Interfaces;
using Zord.Repository.EntityFrameworkCore;

namespace Sample.Data.Persistence
{
    public class AppRepository<TEntity> : RepositoryBase<TEntity>, IAppRepository<TEntity>
        where TEntity : class, IEntity
    {
        public AppRepository(AlphaDbContext context) : base(context)
        {
        }
    }
}
