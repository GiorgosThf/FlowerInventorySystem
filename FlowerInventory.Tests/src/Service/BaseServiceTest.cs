using System.Linq.Expressions;
using FlowerInventorySystem.FlowerInventory.Web.Model;
using FlowerInventorySystem.FlowerInventory.Web.Repository;
using FlowerInventorySystem.FlowerInventory.Web.Service;
using JetBrains.Annotations;
using Moq;

namespace FlowerInventory.Tests.Service;

[TestSubject(typeof(BaseService<>))]
public class BaseServiceTests
{

    public class TestEntity : BaseModel
    {
        public string Name { get; init; } = string.Empty;
    }

    private readonly Mock<IBaseRepository<TestEntity>> _repo = new();

    private BaseService<TestEntity> CreateSut() => new(_repo.Object);

    [Fact]
    public async Task GetAllAsync_DelegatesToRepository_AndReturnsResult()
    {
        // Arrange
        var expected = new List<TestEntity> { new() { Id = 1, Name = "A" } };

        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var sut = CreateSut();

        // Act
        var result = await sut.GetAllAsync(CancellationToken.None);
        
        // Assert
        Assert.Same(expected, result);

        _repo.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_DelegatesToRepository_AndReturnsEntity()
    {
        //Arrange
        var entity = new TestEntity { Id = 42, Name = "X" };
        _repo.Setup(r => r.GetByIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        var sut = CreateSut();

        //Act
        var result = await sut.GetByIdAsync(42, CancellationToken.None);
        
        //Assert
        Assert.Same(entity, result);
        _repo.Verify(r => r.GetByIdAsync(42, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindAsync_DelegatesToRepository_WithSamePredicate()
    {
        Expression<Func<TestEntity, bool>> predicate = e => e.Name == "rose";
        var expected = new List<TestEntity> { new() { Id = 7, Name = "rose" } };

        _repo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<TestEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var sut = CreateSut();

        var result = await sut.FindAsync(predicate, CancellationToken.None);

        Assert.Same(expected, result);
        _repo.Verify(r => r.FindAsync(predicate, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DelegatesToRepository_AndReturnsCreated()
    {
        var input = new TestEntity { Id = 0, Name = "new" };
        var created = new TestEntity { Id = 10, Name = "new" };

        _repo.Setup(r => r.CreateAsync(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(created);

        var sut = CreateSut();

        var result = await sut.CreateAsync(input, CancellationToken.None);

        Assert.Same(created, result);
        _repo.Verify(r => r.CreateAsync(input, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DelegatesToRepository()
    {
        var entity = new TestEntity { Id = 5, Name = "edit" };

        _repo.Setup(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = CreateSut();

        await sut.UpdateAsync(entity, CancellationToken.None);

        _repo.Verify(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DelegatesToRepository()
    {
        var entity = new TestEntity { Id = 9, Name = "del" };

        _repo.Setup(r => r.DeleteAsync(entity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = CreateSut();

        await sut.DeleteAsync(entity, CancellationToken.None);

        _repo.Verify(r => r.DeleteAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryThrows_LoggerStillCalled_AndExceptionBubbles()
    {
        _repo.Setup(r => r.GetByIdAsync(123, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetByIdAsync(123, CancellationToken.None));

    }
}