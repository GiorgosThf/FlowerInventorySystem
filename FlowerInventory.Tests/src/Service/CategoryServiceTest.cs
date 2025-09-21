using FlowerInventorySystem.FlowerInventory.Web.Model;
using FlowerInventorySystem.FlowerInventory.Web.Repository;
using FlowerInventorySystem.FlowerInventory.Web.Service;
using JetBrains.Annotations;
using Moq;

namespace FlowerInventory.Tests.Service;

[TestSubject(typeof(CategoryService))]
public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _repo = new();
        private CategoryService CreateSut() => new(_repo.Object);

        [Fact]
        public async Task GetByPublicIdAsync_DelegatesToRepo_AndLogs()
        {
            
            var id = Guid.NewGuid();
            var expected = new Category { Id = 1, PublicId = id, Name = "Roses" };
            _repo.Setup(r => r.GetByPublicIdAsync(id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(expected);

            var sut = CreateSut();
            var result = await sut.GetByPublicIdAsync(id, TestContext.Current.CancellationToken);

            Assert.Same(expected, result);
            _repo.Verify(r => r.GetByPublicIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("ros", "name")]
        [InlineData("ros", "name_desc")]
        public async Task SearchAsync_DelegatesToRepo_AndLogs(string q, string sort)
        {
            var expected = new List<Category> { new() { Id = 1, Name = "Roses" } };
            _repo.Setup(r => r.SearchAsync(q, sort, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(expected);

            var sut = CreateSut();
            var result = await sut.SearchAsync(q, sort, TestContext.Current.CancellationToken);

            Assert.Same(expected, result);
            _repo.Verify(r => r.SearchAsync(q, sort, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetWithFlowersAsync_DelegatesToRepo_AndLogs()
        {
            _repo.Setup(r => r.GetWithFlowersAsync(5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new Category { Id = 5, Name = "Tulips" });

            var sut = CreateSut();
            var result = await sut.GetWithFlowersAsync(5, TestContext.Current.CancellationToken);

            Assert.NotNull(result);
            _repo.Verify(r => r.GetWithFlowersAsync(5, It.IsAny<CancellationToken>()), Times.Once);
        }
}
