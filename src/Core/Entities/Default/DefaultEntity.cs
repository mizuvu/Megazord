namespace Zord.Entities.Default;

/// <summary>
///     A base class for DDD Entities with default ID using MassTransit.NewId
/// </summary>
public abstract class BaseEntity : BaseEntity<string>
{
    protected BaseEntity() => Id = MassTransit.NewId.Next().ToString();
}

/// <summary>
///     A base class for DDD Auditable Entities with default ID using MassTransit.NewId
/// </summary>
public abstract class AuditableEntity : AuditableEntity<string>
{
    protected AuditableEntity() => Id = MassTransit.NewId.Next().ToString();
}