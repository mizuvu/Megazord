using Microsoft.AspNetCore.Identity;
using Zord.Core.Entities.Interfaces;
using Zord.Core.ValueObjects;

namespace Zord.Identity.EntityFrameworkCore.Models;

public class ApplicationUser : IdentityUser, IEntity, IAuditableEntity, ISoftDelete
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public bool UseDomainPassword { get; set; }

    public Status Status { get; set; } = new();

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }
}
