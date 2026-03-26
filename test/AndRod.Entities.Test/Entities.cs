using AndRod.StronglyTypedIds;

namespace AndRod.Entities.Test;

public sealed record SomeEntityId(Guid Value) : StronglyTypedId<SomeEntityId, Guid>(Value);

public sealed class SomeEntity : AbstractEntity<SomeEntity, SomeEntityId>
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
