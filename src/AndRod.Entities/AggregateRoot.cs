using AndRod.StronglyTypedIds;

namespace AndRod.Entities;

/// <summary>
/// This class represents the base class for aggregate roots in a domain-driven design context. An aggregate root is an entity that serves as the entry point for a cluster of related entities and value objects. It ensures the integrity of the aggregate by enforcing invariants and controlling access to its members.
/// </summary>
public abstract class AggregateRoot<TAggregateRoot, TId> : Entity<TAggregateRoot, TId>,
    IAggregateRoot<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TAggregateRoot, TId>
    where TId : IStronglyTypedId, IComparable<TId>
{
    /// <summary>
    /// Creates a new instance of the <see cref="AggregateRoot{TAggregateRoot, TId}"/> class with a default identifier and the current utc date and time as the creation timestamp.
    /// </summary>
    protected AggregateRoot() : base()
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AggregateRoot{TAggregateRoot, TId}"/> class with the specified identifier and the current utc date and time as the creation timestamp.
    /// </summary>
    protected AggregateRoot(TId id) : base(id, DateTimeOffset.UtcNow)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AggregateRoot{TAggregateRoot, TId}"/> class with the specified identifier, creation timestamp, and optional update timestamp.
    /// </summary>
    protected AggregateRoot(TId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null) : base(id, createdAt, updatedAt)
    {
    }

    private readonly Queue<IDomainEvent> _domainEvents = [];

    /// <inheritdoc/>
    protected void EnqueueDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }

    /// <inheritdoc/>
    public bool HasDomainEvents => _domainEvents.Count > 0;

    /// <summary>
    /// Enqueues a domain event to be processed later. This method is typically called by the aggregate root when a significant change occurs that should be communicated to other parts of the system. The enqueued domain events can be processed by an event handler or a message bus to trigger side effects or update read models.
    /// </summary>
    public IEnumerable<IDomainEvent> DequeueDomainEvents()
    {
        while (HasDomainEvents)
        {
            yield return _domainEvents.Dequeue();
        }
    }
}

/// <summary>
/// Represents the contract for an aggregate root in a domain-driven design context.
/// An aggregate root is an entity that serves as the entry point for a cluster of related entities and value objects. It ensures the integrity of the aggregate by enforcing invariants and controlling access to its members.
/// </summary>
public interface IAggregateRoot<TSelf, TKey> : IEntity<TSelf, TKey>, IAggregateRoot
    where TSelf : IAggregateRoot<TSelf, TKey>
    where TKey : IStronglyTypedId
{
}

/// <summary>
/// Represents the contract for an aggregate root in a domain-driven design context.
/// An aggregate root is an entity that serves as the entry point for a cluster of related entities and value objects. It ensures the integrity of the aggregate by enforcing invariants and controlling access to its members.
/// </summary>
public interface IAggregateRoot : IEntity
{
    /// <summary>
    /// Gets a value indicating whether the aggregate root has any domain events that have been enqueued. This property is typically used to determine if there are any pending domain events that need to be processed before saving the aggregate root to the database or publishing the events to an event bus.
    /// </summary>
    public bool HasDomainEvents { get; }

    /// <summary>
    /// Dequeues all domain events that have been enqueued by the aggregate root.
    /// </summary>
    IEnumerable<IDomainEvent> DequeueDomainEvents();
}