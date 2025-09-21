using FlowerInventory.Web.Model;

namespace FlowerInventory.Web.Repository;

public interface IFlowerRepository : IBaseRepository<Flower>
{
    Task<Flower?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default);

    Task<List<Flower>> ListByCategoryPublicIdAsync(Guid categoryPublicId, CancellationToken ct = default);

    Task<List<Flower>> ListWithCategoryAsync(CancellationToken ct = default);

    Task<List<Flower>> SearchAsync(string? q, int? categoryId, string? sort, CancellationToken ct = default);
}