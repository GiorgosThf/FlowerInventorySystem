using FlowerInventory.Web.Model;
using FlowerInventory.Web.Repository;

namespace FlowerInventory.Web.Service;
/* The Flower Service extending BaseService with basic CRUD operations and implementing other operations
 from FlowerService Interface*/
public class FlowerService(IFlowerRepository flowerRepository)
    : BaseService<Flower>(flowerRepository), IFlowerService
{
    /* Search for a service based on a query, sort based on field */
    public Task<List<Flower>> SearchAsync(string? q, int? categoryId, string? sort, CancellationToken ct = default)
    {
        Logger.LogInformation("SearchAsync {@q}", q);
        return flowerRepository.SearchAsync(q, categoryId, sort, ct);
    }

    /* Fetch flowers by public id */
    public Task<Flower?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default)
    {
        Logger.LogInformation("GetByPublicIdAsync {@publicId}", publicId);
        return flowerRepository.GetByPublicIdAsync(publicId, ct);
    }

    /* Fetch flowers by category public id  */
    public Task<List<Flower>> GetByCategoryPublicIdAsync(Guid categoryPublicId, CancellationToken ct = default)
    {
        Logger.LogInformation("GetByCategoryPublicIdAsync {@categoryPublicId}", categoryPublicId);
        return flowerRepository.ListByCategoryPublicIdAsync(categoryPublicId, ct);
    }

    /* Fetch with category (lazy loading) */
    public Task<List<Flower>> ListWithCategoryAsync(CancellationToken ct = default)
    {
        Logger.LogInformation("ListWithCategoryAsync");
        return flowerRepository.ListWithCategoryAsync(ct);
    }
}