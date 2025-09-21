using System.Linq.Expressions;
using FlowerInventory.Web.Model;

namespace FlowerInventory.Web.Service;

/* The Interface of Base Service with basic CRUD operations*/
public interface IBaseService<T> where T : BaseModel
{
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T> CreateAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
}