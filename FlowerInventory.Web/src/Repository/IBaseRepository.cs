using System.Linq.Expressions;
using FlowerInventorySystem.FlowerInventory.Web.Model;

namespace FlowerInventorySystem.FlowerInventory.Web.Repository;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T> CreateAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
}