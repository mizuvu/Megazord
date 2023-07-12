using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zord.Core.Entities.Interfaces;
using Zord.Core.ValueObjects;
using Zord.Identity.EntityFrameworkCore.Data;

namespace Zord.Identity.Migrator.Data;

public class AppIdentityDbContext : IdentityDbContext
{
    public AppIdentityDbContext(
        DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable(name: "Users", Schema.Identity);
        builder.Entity<IdentityUserLogin<string>>().ToTable(name: "UserLogins", Schema.Identity);
        builder.Entity<IdentityUserClaim<string>>().ToTable(name: "UserClaims", Schema.Identity);
        builder.Entity<IdentityUserToken<string>>().ToTable(name: "UserTokens", Schema.Identity);
        builder.Entity<ApplicationRole>().ToTable(name: "Roles", Schema.Identity);
        builder.Entity<IdentityRoleClaim<string>>().ToTable(name: "RoleClaims", Schema.Identity);
        builder.Entity<ApplicationUserRole>().ToTable(name: "UserRoles", Schema.Identity);

        builder.Entity<JwtToken>(e =>
        {
            e.HasKey(x => x.UserId);
            e.ToTable(name: "JwtTokens", Schema.Identity);
        });
    }

    // add audit log info to entities
    private void AuditEntities()
    {
        // this function for fix null values Entities use ISoftDelete & ValueObjects
        foreach (var entry in ChangeTracker.Entries<ValueObject>()
            .Where(x => x.State is EntityState.Deleted)
            .ToList())
        {
            entry.State = EntityState.Unchanged;
        }

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTimeOffset.Now;
                    entry.Entity.CreatedBy = "";
                    entry.Entity.LastModifiedOn = DateTimeOffset.Now;
                    entry.Entity.LastModifiedBy = "";
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTimeOffset.Now;
                    entry.Entity.LastModifiedBy = "";
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.IsDeleted = true;
                        softDelete.DeletedOn = DateTimeOffset.Now;
                        softDelete.DeletedBy = "";

                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }
    }
}
