using AndRod.StronglyTypedIds;

using Microsoft.Extensions.DependencyInjection;

namespace AndRod.Entities.Test;

[TestClass]
public sealed class TestAggregateRootClass
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStronglyTypedIds(config => config.Add<SomeAggregateRootId>());
    }

    [TestMethod]
    public void Create_Empty_SomeAggregateRoot_Should_Create_With_Current_Time()
    {
        var dateTime = DateTimeOffset.Now;

        var aggregateRoot = new SomeAggregateRoot(SomeAggregateRootId.Empty(), dateTime);

        Assert.AreEqual(Guid.Empty, aggregateRoot.Id.Value);
        Assert.AreEqual(dateTime, aggregateRoot.CreatedAt);
        Assert.IsFalse(aggregateRoot.UpdatedAt.HasValue);
        Assert.IsNull(aggregateRoot.SomeEntity);
    }

    [TestMethod]
    public void Create_SomeAggregateRoot_With_Id_Should_Create_With_Given_Id()
    {
        var value = Guid.NewGuid();
        var id = new SomeAggregateRootId(value);
        var dateTime = DateTimeOffset.Now;

        var aggregateRoot = new SomeAggregateRoot(id, dateTime);

        Assert.AreEqual(id, aggregateRoot.Id);
        Assert.AreEqual(dateTime, aggregateRoot.CreatedAt);
        Assert.IsFalse(aggregateRoot.UpdatedAt.HasValue);
        Assert.IsNull(aggregateRoot.SomeEntity);
    }

    [TestMethod]
    public void Create_SomeAggregateRoot_With_Id_And_Entity_Should_Create_With_Given_Id_And_Entity()
    {
        var value = Guid.NewGuid();
        var id = new SomeAggregateRootId(value);
        var dateTime = DateTimeOffset.Now;
        var entity = new SomeEntity(SomeEntityId.Empty(), dateTime);

        var aggregateRoot = new SomeAggregateRoot(id, dateTime, null, entity);

        Assert.AreEqual(id, aggregateRoot.Id);
        Assert.AreEqual(dateTime, aggregateRoot.CreatedAt);
        Assert.IsFalse(aggregateRoot.UpdatedAt.HasValue);
        Assert.IsNotNull(aggregateRoot.SomeEntity);
    }

    [TestMethod]
    public void Clone_AggregateRoot_Should_Create_Clone_With_Same_Id_And_Dates()
    {
        var value = Guid.NewGuid();
        var id = new SomeAggregateRootId(value);
        var dateTime = DateTimeOffset.Now;
        var aggregateRoot = new SomeAggregateRoot(id, dateTime);

        var clone = aggregateRoot.Clone();

        Assert.AreEqual(aggregateRoot.Id, clone.Id);
        Assert.AreEqual(aggregateRoot.CreatedAt, clone.CreatedAt);
        Assert.AreEqual(aggregateRoot.UpdatedAt, clone.UpdatedAt);
        Assert.IsNull(clone.SomeEntity);
    }

    [TestMethod]
    public void Clone_AggregateRoot_With_Entity_Should_Create_Clone_With_Same_Id_Dates_And_Cloned_Entity()
    {
        var value = Guid.NewGuid();
        var id = new SomeAggregateRootId(value);
        var dateTime = DateTimeOffset.Now;
        var entity = new SomeEntity(SomeEntityId.Empty(), dateTime);
        var aggregateRoot = new SomeAggregateRoot(id, dateTime, null, entity);

        var clone = aggregateRoot.Clone();

        Assert.AreEqual(aggregateRoot.Id, clone.Id);
        Assert.AreEqual(aggregateRoot.CreatedAt, clone.CreatedAt);
        Assert.AreEqual(aggregateRoot.UpdatedAt, clone.UpdatedAt);
        Assert.IsNotNull(clone.SomeEntity);
        Assert.AreEqual(aggregateRoot.SomeEntity?.Id, clone.SomeEntity?.Id);
        Assert.AreEqual(aggregateRoot.SomeEntity?.CreatedAt, clone.SomeEntity?.CreatedAt);
        Assert.AreEqual(aggregateRoot.SomeEntity?.UpdatedAt, clone.SomeEntity?.UpdatedAt);
        Assert.AreNotSame(aggregateRoot, clone);
        Assert.AreNotSame(aggregateRoot.SomeEntity, clone.SomeEntity);
    }
}