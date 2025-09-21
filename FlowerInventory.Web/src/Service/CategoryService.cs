using FlowerInventory.Web.Model;
using FlowerInventory.Web.Repository;

namespace FlowerInventory.Web.Service;

/* The Category Service extending BaseService with basic CRUD operations and implementing other operations
 from CategoryService Interface*/
public class CategoryService(ICategoryRepository categoryRepository)
    : BaseService<Category>(categoryRepository), ICategoryService
{
    /* Search for a category based on a query, sort based on field */
    public Task<List<Category>> SearchAsync(string? q, string? sort, CancellationToken ct = default)
    {
        Logger.LogInformation("SearchAsync {q} ", q);
        return categoryRepository.SearchAsync(q, sort, ct);
    }

    /* Fetch category with flowers (lazy loading) */
    public Task<Category?> GetWithFlowersAsync(int id, CancellationToken ct = default)
    {
        Logger.LogInformation("GetWithFlowersAsync {id} ", id);
        return categoryRepository.GetWithFlowersAsync(id, ct);
    }

    /* Get Category by Public id (future use) */
    public Task<Category?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default)
    {
        Logger.LogInformation("GetByPublicIdAsync {publicId} ", publicId);
        return categoryRepository.GetByPublicIdAsync(publicId, ct);
    }
}