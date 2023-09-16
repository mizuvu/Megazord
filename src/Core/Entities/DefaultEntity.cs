namespace Zord.Entities;

/// <summary>
///     A base class for DDD Entities with default ID using MassTransit.NewId
/// </summary>
public abstract class DefaultEntityBase : BaseEntity<string>
{
    protected DefaultEntityBase() => Id = MassTransit.NewId.Next().ToString();
}

/// <summary>
///     A base class for DDD Auditable Entities with default ID using MassTransit.NewId
/// </summary>
public abstract class DefaultAuditableEntityBase : BaseAuditableEntity<string>
{
    protected DefaultAuditableEntityBase() => Id = MassTransit.NewId.Next().ToString();
}