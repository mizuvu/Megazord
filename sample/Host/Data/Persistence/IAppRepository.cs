using Zord.Core.Entities.Interfaces;
using Zord.Core.Repositories;

namespace Host.Data.Persistence
{
    public interface IAppRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
