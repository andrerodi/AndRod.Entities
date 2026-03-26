using AndRod.StronglyTypedIds;

namespace AndRod.Entities;

public interface IAbstractEntity<TSelf, TKey>
    where TSelf : IAbstractEntity<TSelf, TKey>
    where TKey : IStronglyTypedId
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    TKey Id { get; }

    /// <summary>
    /// Updates the entity's <see cref="UpdatedAt"/> timestamp.
    /// </summary>
    public void Update();
}
