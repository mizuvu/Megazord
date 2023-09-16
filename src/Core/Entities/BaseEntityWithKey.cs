using Zord.Entities.Interfaces;

namespace Zord.Entities;

/// <summary>
///     A base class for DDD Entities. Includes support for domain events dispatched post-persistence.
///     support both GUID and int IDs, change to EntityBase&lt;TId&gt; and use TId as the type for Id.
/// </summary>
public abstract class BaseEntity<TId> : BaseEvent, IEntity<TId>
{
    public virtual TId Id { get; protected set; } = default!;
}

/// <summary>
///     A base class for DDD Auditable Entities. Includes support for domain events dispatched post-persistence.
///     support both GUID and int IDs, change to EntityBase&lt;TId&gt; and use TId as the type for Id.
/// </summary>
public abstract class BaseAuditableEntity<TId> : BaseAuditableEntity, IEntity<TId>, IAuditableEntity
{
    public virtual TId Id { get; protected set; } = default!;
}