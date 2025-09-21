using FlowerInventory.Web.Configuration;
using FlowerInventory.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace FlowerInventory.Web.Repository;

public class FlowerRepository(AppDbContext db)
    : BaseRepository<Flower>(db), IFlowerRepository
{
    public Task<Flower?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default)
    {
        Logger.LogInformation("GetByPublicId {}", publicId);
        return Set.Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.PublicId == publicId, ct);
    }

    public Task<List<Flower>> ListByCategoryPublicIdAsync(Guid categoryPublicId, CancellationToken ct = default)
    {
        Logger.LogInformation("ListByCategoryPublicId {}", categoryPublicId);
        return Set.Include(f => f.Category)
            .Where(f => f.Category != null && f.Category.PublicId == categoryPublicId)
            .OrderBy(f => f.Id)
            .ToListAsync(ct);
    }

    public Task<List<Flower>> ListWithCategoryAsync(CancellationToken ct = default)
    {
        Logger.LogInformation("ListWithCategoryAsync");
        return Set.Include(f => f.Category)
            .OrderBy(f => f.Id)
            .ToListAsync(ct);
    }

    public async Task<List<Flower>> SearchAsync(string? q, int? categoryId, string? sort,
        CancellationToken ct = default)
    {
        Logger.LogInformation("SearchAsync {}", q);

        var query = Set.Include(f => f.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var t = q.ToLower();
            query = query.Where(f => f.Name.ToLower().Contains(t) || f.Type.ToLower().Contains(t));
        }

        if (categoryId.HasValue) query = query.Where(f => f.CategoryId == categoryId.Value);

        query = (sort ?? "").ToLower() switch
        {
            "name" => query.OrderBy(f => f.Name),
            "name_desc" => query.OrderByDescending(f => f.Name),
            "type" => query.OrderBy(f => f.Type),
            "type_desc" => query.OrderByDescending(f => f.Type),
            "price" => query.OrderBy(f => f.Price),
            "price_desc" => query.OrderByDescending(f => f.Price),
            "category" => query.OrderBy(f => f.Category!.Name),
            "category_desc" => query.OrderByDescending(f => f.Category!.Name),
            _ => query.OrderBy(f => f.Id)
        };

        return await query.ToListAsync(ct);
    }
}