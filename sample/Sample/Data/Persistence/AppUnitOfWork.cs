using Sample.Data;
using Zord.Repository.EntityFrameworkCore;

namespace Sample.Data.Persistence
{
    public class AppUnitOfWork : UnitOfWorkBase, IAppUnitOfWork
    {
        public AppUnitOfWork(AlphaDbContext context) : base(context)
        {
        }
    }
}
