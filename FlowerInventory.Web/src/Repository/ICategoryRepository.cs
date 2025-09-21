using FlowerInventorySystem.FlowerInventory.Web.Model;

namespace FlowerInventorySystem.FlowerInventory.Web.Repository;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<List<Category>> SearchAsync(string? q, string? sort, CancellationToken ct = default);
    Task<Category?> GetWithFlowersAsync(int id, CancellationToken ct = default);
}