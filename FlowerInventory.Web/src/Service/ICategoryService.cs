using FlowerInventory.Web.Model;

namespace FlowerInventory.Web.Service;

public interface ICategoryService : IBaseService<Category>
{
    Task<List<Category>> SearchAsync(string? q, string? sort, CancellationToken ct = default);
    Task<Category?> GetWithFlowersAsync(int id, CancellationToken ct = default);
}