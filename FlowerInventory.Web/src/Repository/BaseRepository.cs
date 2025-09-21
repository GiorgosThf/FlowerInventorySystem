using System.Linq.Expressions;
using FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace FlowerInventorySystem.FlowerInventory.Web.Repository;

public class BaseRepository<T>(AppDbContext db)
    : BaseComponent<BaseRepository<T>>, IBaseRepository<T> where T : BaseModel
{
    protected readonly DbSet<T> Set = db.Set<T>();

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        Logger.LogInformation("Get all {}", typeof(T).Name);
        return await Set.ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        Logger.LogInformation("GetById {}", typeof(T).Name);
        return await Set.FindAsync([id], ct);
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        Logger.LogInformation("Find {}", typeof(T).Name);
        return await Set.Where(predicate).ToListAsync(ct);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken ct = default)
    {
        Logger.LogInformation("Create {}", typeof(T).Name);
        Set.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        Logger.LogInformation("Update {}", typeof(T).Name);
        Set.Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        Logger.LogInformation("Delete {}", typeof(T).Name);
        Set.Remove(entity);
        await db.SaveChangesAsync(ct);
    }
}