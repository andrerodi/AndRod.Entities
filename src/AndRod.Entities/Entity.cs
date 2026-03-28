using AndRod.Core;
using AndRod.StronglyTypedIds;

namespace AndRod.Entities;

/// <summary>
/// Represents an abstract entity with a strongly-typed ID.
/// </summary>
/// <typeparam name="TSelf">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's ID.</typeparam>
public abstract class Entity<TSelf, TId> : IEntity<TSelf, TId>, ITypedCloneable<TSelf>, IEquatable<TSelf>, IComparable<TSelf>
    where TSelf : Entity<TSelf, TId>
    where TId : IStronglyTypedId, IComparable<TId>
{
    /// <summary>
    /// Creates a new instance of the <see cref="Entity{TSelf, TId}"/> class with a default identifier and the current utc date and time as the creation timestamp.
    /// </summary>
    protected Entity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Entity{TSelf, TId}"/> class with the specified identifier and the current utc date and time as the creation timestamp.
    /// </summary>
    protected Entity(TId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <inheritdoc/>
    public TId Id { get; private set; } = StronglyTypedIdFactory.Empty<TId>();

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <inheritdoc />
    public void Update() => UpdatedAt = DateTimeOffset.UtcNow;

    /// <summary>
    /// Compares the entity's ID to another entity's ID.
    /// </summary>
    /// <inheritdoc />
    public virtual int CompareTo(TSelf? other) => other is null ? 1 : Id.CompareTo(other.Id);

    /// <inheritdoc />
    public abstract TSelf Clone();

    /// <summary>
    /// Compares the entity's ID to another entity's ID for equality.
    /// </summary>
    /// <inheritdoc />
    public bool Equals(TSelf? other) => other is not null && Id.Equals(other.Id);

    /// <summary>
    /// Uses <see cref="TSelf.Equals(TSelf?)"/> to compare the entity's ID to another entity's ID for equality.
    /// </summary>
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is TSelf other && Equals(other);

    /// <summary>
    /// Returns the hash code of the entity's ID.
    /// </summary>
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Returns a string representation of the entity in the format "<see cref="TSelf"/>: <see cref="Id"/>".
    /// </summary>
    public override string ToString() => $"{typeof(TSelf).Name}: {Id} - Created: {CreatedAt} - Updated: {FormatDateTime(UpdatedAt)}";

    private static string FormatDateTime(DateTimeOffset? dateTime) => dateTime.HasValue ? dateTime.Value.ToString() : "N/A";
}

/// <summary>
/// Represents the contract for an abstract entity in a domain-driven design context.
/// </summary>
public interface IEntity<TSelf, TKey> : IEntity
    where TSelf : IEntity<TSelf, TKey>
    where TKey : IStronglyTypedId
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    TKey Id { get; }
}

/// <summary>
/// Represents the contract for an abstract entity in a domain-driven design context.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets the timestamp when the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when the entity was last updated.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; }

    /// <summary>
    /// Updates the entity's <see cref="UpdatedAt"/> timestamp.
    /// </summary>
    public void Update();
}