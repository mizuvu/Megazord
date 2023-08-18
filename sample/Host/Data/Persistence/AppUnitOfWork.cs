namespace Host.Data.Persistence
{
    public class AppUnitOfWork : UnitOfWorkBase, IAppUnitOfWork
    {
        public AppUnitOfWork(AlphaDbContext context) : base(context)
        {
        }
    }
}
