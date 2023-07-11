namespace Zord.EntityFrameworkCore;

public class UnitOfWork<TContext> : UnitOfWorkBase, IUnitOfWork<TContext>
    where TContext : DbContext
{
    public UnitOfWork(TContext context) : base(context)
    {
    }
}
