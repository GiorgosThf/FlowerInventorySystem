using System.Linq.Expressions;
using FlowerInventory.Web.Configuration;
using FlowerInventory.Web.Model;
using FlowerInventory.Web.Repository;

namespace FlowerInventory.Web.Service;
/* The Base Service with basic CRUD operations*/
public class BaseService<T>(IBaseRepository<T> repository)
    : BaseComponent<IBaseRepository<T>>, IBaseService<T> where T : BaseModel
{
    /* Find All Objects */
    public Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        Logger.LogInformation("GetAllAsync {}", typeof(T).Name);
        return repository.GetAllAsync(ct);
    }

    /* Find Object By id */
    public Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        Logger.LogInformation("GetByIdAsync {id} {}", id, typeof(T).Name);
        return repository.GetByIdAsync(id, ct);
    }

    /* Find Object By a predicate e.g Name = "something" */
    public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        Logger.LogInformation("FindAsync {predicate} {}", predicate.Body.ToString(), typeof(T).Name);
        return repository.FindAsync(predicate, ct);
    }

    /* Create an object */
    public Task<T> CreateAsync(T entity, CancellationToken ct = default)
    {
        Logger.LogInformation("CreateAsync {entity} {}", entity.ToString(), typeof(T).Name);
        return repository.CreateAsync(entity, ct);
    }

    /* Update an object */
    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        Logger.LogInformation("UpdateAsync {entity} {}", entity.ToString(), typeof(T).Name);
        return repository.UpdateAsync(entity, ct);
    }

    /* Delete an object */
    public Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        Logger.LogInformation("DeleteAsync {entity} {}", entity.ToString(), typeof(T).Name);
        return repository.DeleteAsync(entity, ct);
    }
}