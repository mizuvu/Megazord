namespace Zord.Repository.EntityFrameworkCore;

/// <inheritdoc/>
public class UnitOfWork<TContext>(TContext context) :
    UnitOfWorkBase(context),
    IUnitOfWork<TContext>
    where TContext : DbContext
{
}
