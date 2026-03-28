using AndRod.StronglyTypedIds;

namespace AndRod.Entities.Test;

/// SOME ENTITY
public sealed record SomeEntityId(Guid Value) : StronglyTypedId<SomeEntityId, Guid>(Value);

public sealed class SomeEntity : Entity<SomeEntity, SomeEntityId>
{
    public SomeEntity()
    {
    }

    public SomeEntity(SomeEntityId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null)
    : base(id, createdAt, updatedAt)
    {
    }

    public override SomeEntity Clone() => new(Id, CreatedAt, UpdatedAt);
}

/// SOME HISTORICAL ENTITY
public sealed record SomeHistoricalEntityId(Guid Value) : StronglyTypedId<SomeHistoricalEntityId, Guid>(Value);

public sealed class SomeHistoricalEntity : HistoricalEntity<SomeHistoricalEntity, SomeHistoricalEntityId>
{
    public SomeHistoricalEntity()
    {
    }

    public SomeHistoricalEntity(SomeHistoricalEntityId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null)
    : base(id, createdAt, updatedAt)
    {
    }

    public void UpdateTimestamp()
    {
        var old = UpdatedAt;
        Update();
        AddHistoricalRecord(nameof(UpdatedAt), old, UpdatedAt);
    }

    public override SomeHistoricalEntity Clone() => new(Id, CreatedAt, UpdatedAt);
}

/// SOME AGGREGATE ROOT
public sealed record SomeAggregateRootId(Guid Value) : StronglyTypedId<SomeAggregateRootId, Guid>(Value);

public sealed class SomeAggregateRoot : AggregateRoot<SomeAggregateRoot, SomeAggregateRootId>
{
    public SomeAggregateRoot()
    {
    }

    public SomeAggregateRoot(SomeAggregateRootId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt = null)
    : base(id, createdAt, updatedAt)
    {
    }

    public SomeAggregateRoot(SomeAggregateRootId id, DateTimeOffset createdAt, DateTimeOffset? updatedAt, SomeEntity? someEntity)
        : base(id, createdAt, updatedAt)
    {
        SomeEntity = someEntity;
    }

    public SomeEntity? SomeEntity { get; private set; }

    public override SomeAggregateRoot Clone() => new(Id, CreatedAt, UpdatedAt, SomeEntity?.Clone());
}