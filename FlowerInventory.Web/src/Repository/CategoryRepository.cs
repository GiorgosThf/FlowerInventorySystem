using FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace FlowerInventorySystem.FlowerInventory.Web.Repository;

public class CategoryRepository(AppDbContext db)
    : BaseRepository<Category>(db), ICategoryRepository
{
    public Task<Category?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default)
    {
        Logger.LogInformation("GetByPublicId {}", publicId);
        return Set.Include(f => f.Flowers)
            .FirstOrDefaultAsync(f => f.PublicId == publicId, ct);
    }

    public Task<Category?> GetWithFlowersAsync(int id, CancellationToken ct = default)
    {
        Logger.LogInformation("GetWithFlowersAsync {}", id);
        return Set.Include(c => c.Flowers.OrderBy(f => f.Name))
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<List<Category>> SearchAsync(string? q, string? sort, CancellationToken ct = default)
    {
        Logger.LogInformation("SearchAsync {}", q);
        var query = Set
            .Include(c => c.Flowers.OrderBy(f => f.Name))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var t = q.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(t) || (c.Description ?? "").ToLower().Contains(t));
        }

        query = (sort ?? "").ToLower() switch
        {
            "name" => query.OrderBy(c => c.Name),
            "name_desc" => query.OrderByDescending(c => c.Name),
            "count" => query.OrderBy(c => c.Flowers.Count),
            "count_desc" => query.OrderByDescending(c => c.Flowers.Count),
            _ => query.OrderBy(c => c.Id)
        };

        return await query.ToListAsync(ct);
    }
}