using AndRod.Core;
using AndRod.StronglyTypedIds;

namespace AndRod.Entities;

/// <summary>
/// Represents an abstract entity with a strongly-typed ID.
/// </summary>
/// <typeparam name="TSelf">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's ID.</typeparam>
public abstract class AbstractEntity<TSelf, TId> :
    IAbstractEntity<TSelf, TId>,
    ITypedCloneable<TSelf>,
    IEquatable<TSelf>,
    IComparable<TSelf>
    where TSelf : AbstractEntity<TSelf, TId>
    where TId : IStronglyTypedId, IComparable<TId>
{
    protected AbstractEntity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    protected AbstractEntity(TId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Gets the entity's ID.
    /// </summary>
    public TId Id { get; private set; } = StronglyTypedIdFactory.Empty<TId>();

    /// <summary>
    /// Gets the date and time the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time the entity was last updated.
    /// </summary>
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