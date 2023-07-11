namespace Zord.Identity.EntityFrameworkCore.Abstractions
{
    public interface IIdentityDbContext
    {
        DbSet<ApplicationUserRole> ApplicationUserRoles { get; }
        DbSet<JwtToken> JwtTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
