using FlowerInventorySystem.FlowerInventory.Web.Model;
using FlowerInventorySystem.FlowerInventory.Web.Repository;
using FlowerInventorySystem.FlowerInventory.Web.Service;
using JetBrains.Annotations;
using Moq;

namespace FlowerInventory.Tests.Service;

[TestSubject(typeof(FlowerService))]
public class FlowerServiceTests
{
    private readonly Mock<IFlowerRepository> _repo = new();

    private FlowerService CreateSut()
    {
        return new FlowerService(_repo.Object);
    }

    [Fact]
    public async Task GetByPublicIdAsync_DelegatesToRepo_AndLogs()
    {
        var id = Guid.NewGuid();
        var flower = new Flower { Id = 10, PublicId = id, Name = "Red Rose" };
        _repo.Setup(r => r.GetByPublicIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(flower);

        var sut = CreateSut();
        var result = await sut.GetByPublicIdAsync(id, TestContext.Current.CancellationToken);

        Assert.Same(flower, result);
        _repo.Verify(r => r.GetByPublicIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByCategoryPublicIdAsync_DelegatesToRepo_AndLogs()
    {
        var catId = Guid.NewGuid();
        var list = new List<Flower> { new() { Id = 1, Name = "A" } };
        _repo.Setup(r => r.ListByCategoryPublicIdAsync(catId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var sut = CreateSut();
        var result = await sut.GetByCategoryPublicIdAsync(catId, TestContext.Current.CancellationToken);

        Assert.Same(list, result);
        _repo.Verify(r => r.ListByCategoryPublicIdAsync(catId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListWithCategoryAsync_DelegatesToRepo_AndLogs()
    {
        var list = new List<Flower> { new() { Id = 1, Name = "A" } };
        _repo.Setup(r => r.ListWithCategoryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var sut = CreateSut();
        var result = await sut.ListWithCategoryAsync(TestContext.Current.CancellationToken);

        Assert.Same(list, result);
        _repo.Verify(r => r.ListWithCategoryAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("rose", 1, "price")]
    public async Task SearchAsync_DelegatesToRepo_AndLogs(string q, int? categoryId, string sort)
    {
        var list = new List<Flower> { new() { Id = 1, Name = "R1" } };
        _repo.Setup(r => r.SearchAsync(q, categoryId, sort, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var sut = CreateSut();
        var result = await sut.SearchAsync(q, categoryId, sort, TestContext.Current.CancellationToken);

        Assert.Same(list, result);
        _repo.Verify(r => r.SearchAsync(q, categoryId, sort, It.IsAny<CancellationToken>()), Times.Once);
    }
}