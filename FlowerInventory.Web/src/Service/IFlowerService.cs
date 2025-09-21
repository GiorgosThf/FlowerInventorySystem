using FlowerInventorySystem.FlowerInventory.Web.Model;

namespace FlowerInventorySystem.FlowerInventory.Web.Service;

/* The interface with methods related to flower service */
public interface IFlowerService : IBaseService<Flower>
{
    Task<List<Flower>> SearchAsync(string? q, int? categoryId, string? sort, CancellationToken ct = default);
}