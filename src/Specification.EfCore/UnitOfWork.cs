namespace Zord.Specification.EntityFrameworkCore;

/// <inheritdoc/>
public class UnitOfWork<TContext> : UnitOfWorkBase, IUnitOfWork<TContext>
    where TContext : DbContext
{
    public UnitOfWork(TContext context) : base(context)
    {
    }
}
