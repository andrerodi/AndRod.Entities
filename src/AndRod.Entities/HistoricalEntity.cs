namespace AndRod.Entities;

/// <summary>
/// This contract defines the structure for a historical entity, which is an entity that maintains a collection of historical records capturing changes made to the entity over time. This allows for tracking and auditing of modifications, providing a comprehensive history of changes for the entity.
/// </summary>
public interface IHistoricalEntity
{
    /// <summary>
    /// A collection of historical records that capture changes made to the entity over time. This property allows for tracking and auditing of modifications, providing a comprehensive history of changes for the entity.
    /// </summary>
    HistoryicalRecordCollection History { get; }
}

/// <summary>
/// This class represents a collection of historical records for an entity.
/// </summary>
public sealed class HistoryicalRecordCollection
{
    private HistoryicalRecordCollection() { }

    private readonly List<IHistoricalRecord> _history = [];

    /// <inheritdoc />
    public IReadOnlyList<IHistoricalRecord> History => _history;

    /// <inheritdoc />
    public void AddRecord<T>(string propertyName, T? oldValue, T? newValue)
    {
        var record = new HistoricalRecord<T>(propertyName, oldValue, newValue);
        _history.Add(record);
    }

    public static HistoryicalRecordCollection Create() => new();
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