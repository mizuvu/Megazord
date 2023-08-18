namespace Host.Data.Persistence
{
    public interface IAppRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
