namespace AndRod.Entities;

/// <summary>
/// This record class represents a domain event in a domain-driven design context.
/// A domain event is an occurrence that is significant to the business domain and is used to capture changes or actions that have taken place within the system. It contains a unique identifier and a timestamp indicating when the event occurred.
/// </summary>
public abstract record class DomainEvent : IDomainEvent
{
    /// <inheritdoc />
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <inheritdoc />
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Represents the contract for a domain event in a domain-driven design context.
/// A Domain Event is an occurrence that is significant to the business domain and is used to capture changes or actions that have taken place within the system. It contains a unique identifier and a timestamp indicating when the event occurred.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// The unique identifier of the domain event. This property is initialized with a new version 7 GUID when an instance of the domain event is created.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The timestamp indicating when the domain event occurred. This property is initialized with the current UTC
    /// </summary>
    public DateTimeOffset OccurredAt { get; init; }
}