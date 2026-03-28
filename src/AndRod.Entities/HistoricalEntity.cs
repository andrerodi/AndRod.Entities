using AndRod.StronglyTypedIds;

namespace AndRod.Entities;

/// <summary>
/// Represents an entity that maintains a history of changes made to its properties. This abstract class extends the base Entity class and introduces a mechanism to track changes over time. Each change is recorded as a historical record, which includes the property name, old value, new value, and the timestamp of when the change occurred. This allows for auditing and tracking the evolution of the entity's state throughout its lifecycle.
/// </summary>
public abstract class HistoricalEntity<TSelf, TId> : Entity<TSelf, TId>
    where TSelf : HistoricalEntity<TSelf, TId>
    where TId : IStronglyTypedId, IComparable<TId>
{
    /// <summary>
    /// Creates a new instance of the <see cref="HistoricalEntity{TSelf, TId}"/> class with a default identifier and the current utc date and time as the creation timestamp.
    /// </summary>
    protected HistoricalEntity() : base()
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="HistoricalEntity{TSelf, TId}"/> class with the specified identifier and the current utc date and time as the creation timestamp.
    /// </summary>
    protected HistoricalEntity(TId id) : base(id, DateTimeOffset.UtcNow)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="HistoricalEntity{TSelf, TId}"/> class with the specified identifier, creation timestamp, and optional update timestamp.
    /// </summary>
    protected HistoricalEntity(TId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null) : base(id, createdAt, updatedAt)
    {
    }

    private readonly List<IHistoricalRecord> _history = [];

    /// <inheritdoc />
    public IReadOnlyList<IHistoricalRecord> History => _history;

    /// <inheritdoc />
    public void AddHistoricalRecord<T>(string propertyName, T? oldValue, T? newValue)
    {
        var record = new HistoricalRecord<T>(propertyName, oldValue, newValue);
        _history.Add(record);
    }
}

/// <summary>
/// Represents the contract for an entity that maintains a history of changes made to its properties. This interface defines the structure for tracking changes over time, allowing entities to record historical records that capture the property name, old value, new value, and the timestamp of when the change occurred.
/// </summary>
public interface IHistoricalEntity
{
    /// <summary>
    /// Holds the history of changes made to the entity. Each record in the history represents a change to a specific property, including the old and new values, as well as the timestamp of when the change occurred.
    /// <summary>
    public IReadOnlyList<IHistoricalRecord> History { get; }

    /// <summary>
    /// Adds a historical record to the entity's history. This method is typically called when a property of the entity is changed, allowing the change to be tracked over time. The method takes the name of the property that was changed, the old value before the change, and the new value after the change. It creates a new historical record and adds it to the history list.
    /// </summary>
    void AddHistoricalRecord<T>(string propertyName, T? oldValue, T? newValue);
}

/// <summary>
/// This record class represents a historical record of changes made to a specific property of an entity. It captures the name of the property, the old value before the change, and the new value after the change. Additionally, it includes a timestamp indicating when the change occurred.
/// </summary>
/// <typeparam name="T">The type of the property value that was changed. This allows the historical record to be flexible and accommodate changes to properties of various types, providing a comprehensive history of changes made to the entity.</typeparam>
/// <param name="PropertyName">The name of the property that was changed. This parameter is used to identify which specific property of the entity was modified, allowing for tracking and auditing of changes over time.</param>
/// <param name="OldValue">The old value of the property before the change. </param>
/// <param name="NewValue">The new value of the property after the change. </param>
public record class HistoricalRecord<T>(string PropertyName, T? OldValue, T? NewValue) : IHistoricalRecord
{
    /// <inheritdoc />
    public DateTimeOffset ChangedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// This interface defines the contract for a historical record, which captures changes made to a specific property of an entity. It includes the name of the property that was changed and the timestamp of when the change occurred.
/// </summary>
public interface IHistoricalRecord
{
    /// <summary>
    /// The name of the property that was changed. This property is used to identify which specific property of the entity was modified, allowing for tracking and auditing of changes over time.
    /// </summary>
    string PropertyName { get; init; }

    /// <summary>
    /// The timestamp indicating when the change occurred. This property is used to track the timing of changes made to the entity, providing a historical context for when specific modifications were made.
    /// </summary>
    DateTimeOffset ChangedAt { get; init; }
}