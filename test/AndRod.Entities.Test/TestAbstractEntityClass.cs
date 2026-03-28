using AndRod.StronglyTypedIds;

using Microsoft.Extensions.DependencyInjection;

namespace AndRod.Entities.Test;

[TestClass]
public sealed class TestAbstractEntityClass
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddStronglyTypedIds(config => config.Add<SomeEntityId>());
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

    [TestMethod]
    public void Create_SomeEntity_With_Id_And_Check_With_Another_Entity_With_Same_Id_Should_BeEqual()
    {
        var value = Guid.NewGuid();
        var id1 = new SomeEntityId(value);
        var id2 = new SomeEntityId(value);

        var entity1 = new SomeEntity(id1, DateTimeOffset.Now);
        var entity2 = new SomeEntity(id2, DateTimeOffset.Now);

        Assert.AreEqual(entity1, entity2);
    }

    [TestMethod]
    public void Create_SomeEntity_With_Id_And_Check_WithAnother_Entity_With_Another_Id_Should_NotBeEqual()
    {
        var value1 = Guid.NewGuid();
        var id1 = new SomeEntityId(value1);
        var value2 = Guid.NewGuid();
        var id2 = new SomeEntityId(value2);

        var entity1 = new SomeEntity(id1, DateTimeOffset.Now);
        var entity2 = new SomeEntity(id2, DateTimeOffset.Now);

        Assert.AreNotEqual(entity1, entity2);
    }
}