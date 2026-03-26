using AndRod.StronglyTypedIds;
using Microsoft.Extensions.DependencyInjection;

namespace AndRod.Entities.Test;

[TestClass]
public sealed class Test1
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStronglyTypedIds(config =>
        {
            config.Add<SomeEntityId>();
        });
    }

    [TestMethod]
    public void Create_Empty_SomeEntity_Should_Create_With_Current_Time()
    {
        var dateTime = DateTimeOffset.Now;

        var entity = new SomeEntity(SomeEntityId.Empty(), dateTime);

        Assert.AreEqual(Guid.Empty, entity.Id.Value);
        Assert.AreEqual(dateTime, entity.CreatedAt);
        Assert.IsFalse(entity.UpdatedAt.HasValue);
    }

    [TestMethod]
    public void Create_SomeEntity_With_Id_Should_Create_With_Given_Id()
    {
        var value = Guid.NewGuid();
        var id = new SomeEntityId(value);
        var dateTime = DateTimeOffset.Now;

        var entity = new SomeEntity(id, dateTime);

        Assert.AreEqual(id, entity.Id);
        Assert.AreEqual(dateTime, entity.CreatedAt);
        Assert.IsFalse(entity.UpdatedAt.HasValue);
    }

    [TestMethod]
    public void Update_SomeEntity_Should_Update_UpdatedAt()
    {
        var dateTime = DateTimeOffset.Now;
        var entity = new SomeEntity(SomeEntityId.Empty(), dateTime);

        entity.Update();

        Assert.IsTrue(entity.UpdatedAt.HasValue);
        Assert.AreNotEqual(dateTime, entity.UpdatedAt.Value);
    }

    [TestMethod]
    public void Update_SomeEntity_With_Id_Should_Update_UpdatedAt()
    {
        var dateTime = DateTimeOffset.Now;
        var id = new SomeEntityId(Guid.NewGuid());
        var entity = new SomeEntity(id, dateTime);

        entity.Update();

        Assert.IsTrue(entity.UpdatedAt.HasValue);
        Assert.AreNotEqual(dateTime, entity.UpdatedAt.Value);
    }

    [TestMethod]
    public void Create_Empty_SomeEntity_With_UpdatedAt_Should_Create_Entity_And_Set_UpdatedAt_To_Timestamp()
    {
        var value = Guid.NewGuid();
        var id = new SomeEntityId(value);
        var createdAt = DateTimeOffset.Now;
        var updatedAt = DateTimeOffset.Now;

        var entity = new SomeEntity(id, createdAt, updatedAt);

        Assert.AreEqual(id, entity.Id);
        Assert.AreEqual(createdAt, entity.CreatedAt);
        Assert.IsTrue(entity.UpdatedAt.HasValue);
        Assert.AreNotEqual(createdAt, entity.UpdatedAt);
    }

    [TestMethod]
    public void Create_Empty_SomeEntity_With_UpdatedAt_Should_Create_Entity_And_Set_UpdatedAt_To_Current_Time()
    {
        var value = Guid.NewGuid();
        var id = new SomeEntityId(value);
        var createdAt = DateTimeOffset.Now;
        var updatedAt = DateTimeOffset.Now;

        var entity = new SomeEntity(id, createdAt, updatedAt);
        entity.Update();

        Assert.AreEqual(id, entity.Id);
        Assert.AreEqual(createdAt, entity.CreatedAt);
        Assert.IsTrue(entity.UpdatedAt.HasValue);
        Assert.AreNotEqual(createdAt, entity.UpdatedAt);
        Assert.AreNotEqual(updatedAt, entity.UpdatedAt);
    }
}
