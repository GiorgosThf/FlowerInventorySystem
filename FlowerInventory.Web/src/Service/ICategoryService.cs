using FlowerInventorySystem.FlowerInventory.Web.Model;

namespace FlowerInventorySystem.FlowerInventory.Web.Service;

public interface ICategoryService : IBaseService<Category>
{
    Task<List<Category>> SearchAsync(string? q, string? sort, CancellationToken ct = default);
    Task<Category?> GetWithFlowersAsync(int id, CancellationToken ct = default);
}