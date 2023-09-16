using Zord.Entities.Interfaces;

namespace Zord.Entities;

/// <summary>
///     A base class for DDD Entities. Includes support for domain events dispatched post-persistence.
/// </summary>
public abstract class BaseEntity : BaseEvent, IEntity
{ }

/// <summary>
///     A base class for DDD Auditable Entities. Includes support for domain events dispatched post-persistence.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }
}