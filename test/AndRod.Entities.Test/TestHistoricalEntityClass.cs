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
    public void Create_HistoricalEntity_ShouldInitializeHistory()
    {
        // Arrange
        var entity = new SomeHistoricalEntity();

        // Act
        var history = entity.History;

        // Assert
        Assert.IsNotNull(history);
        Assert.IsEmpty(history.History);
    }

    [TestMethod]
    public void Create_HistoricalEntity_AddRecord_ShouldAddHistoricalRecord()
    {
        // Arrange
        var entity = new SomeHistoricalEntity();

        // Act
        entity.History.AddRecord("Name", "Old Name", "New Name");

        // Assert
        Assert.IsNotEmpty(entity.History.History);
        Assert.HasCount(1, entity.History.History);
        var record = entity.History.History.First();
        Assert.AreEqual("Name", record.PropertyName);
        Assert.AreEqual("Old Name", ((HistoricalRecord<string>)record).OldValue);
        Assert.AreEqual("New Name", ((HistoricalRecord<string>)record).NewValue);
    }
}