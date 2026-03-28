using AndRod.StronglyTypedIds;

using Microsoft.Extensions.DependencyInjection;

namespace AndRod.Entities.Test;

[TestClass]
public sealed class TestHistoricalEntityClass
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStronglyTypedIds(config => config.Add<SomeHistoricalEntityId>());
    }

    [TestMethod]
    public void Create_Empty_SomeHistoricalEntity_Should_Create_With_Current_Time()
    {
        var dateTime = DateTimeOffset.Now;

        var entity = new SomeHistoricalEntity(SomeHistoricalEntityId.Empty(), dateTime);

        Assert.AreEqual(Guid.Empty, entity.Id.Value);
        Assert.AreEqual(dateTime, entity.CreatedAt);
        Assert.IsFalse(entity.UpdatedAt.HasValue);
    }

    [TestMethod]
    public void Create_SomeHistoricalEntity_With_Id_Should_Create_With_Given_Id()
    {
        var value = Guid.NewGuid();
        var id = new SomeHistoricalEntityId(value);
        var dateTime = DateTimeOffset.Now;

        var entity = new SomeHistoricalEntity(id, dateTime);

        Assert.AreEqual(id, entity.Id);
        Assert.AreEqual(dateTime, entity.CreatedAt);
        Assert.IsFalse(entity.UpdatedAt.HasValue);
    }

    [TestMethod]
    public void Update_SomeHistoricalEntity_Should_Update_UpdatedAt()
    {
        var value = Guid.NewGuid();
        var dateTime = DateTimeOffset.Now;
        var entity = new SomeHistoricalEntity(new SomeHistoricalEntityId(value), dateTime);
        var updatedAtBefore = entity.UpdatedAt;

        entity.UpdateTimestamp();
        var history = entity.History.ToArray();
        var historicalRecord = history[0] as HistoricalRecord<DateTimeOffset?>;

        Assert.IsTrue(entity.UpdatedAt.HasValue);
        Assert.AreNotEqual(dateTime, entity.UpdatedAt.Value);
        Assert.HasCount(1, history);
        Assert.IsInstanceOfType<HistoricalRecord<DateTimeOffset?>>(historicalRecord);
        Assert.AreEqual(nameof(SomeHistoricalEntity.UpdatedAt), historicalRecord.PropertyName);
        Assert.AreEqual(updatedAtBefore, historicalRecord.OldValue);
        Assert.AreEqual(entity.UpdatedAt, historicalRecord.NewValue);
    }
}