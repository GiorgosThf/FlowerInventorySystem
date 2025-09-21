using FlowerInventorySystem.FlowerInventory.Web.Dto;
using FlowerInventorySystem.FlowerInventory.Web.Model;

namespace FlowerInventorySystem.FlowerInventory.Web.Service;

/* The Interface with methods related to minio */
public interface IFileStorage
{
    Task<string> UploadAsync(IFormFile file, MediaFolder folder, CancellationToken ct = default);
    Task DeleteAsync(string objectName, CancellationToken ct = default);
    Task<List<StorageObject>> ListAsync(MediaFolder folder, CancellationToken ct = default);
}