using FlowerInventory.Web.Model;
using FlowerInventory.Web.Pages.Utils;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Flowers;

public class Edit(IFlowerService flowers, IFileStorage storage) : PageModel
{
    [BindProperty] public EditInput Input { get; set; } = new();
    [BindProperty] public IFormFile? Image { get; set; }
    [BindProperty] public string? SelectedKey { get; set; }

    public Task<IActionResult> OnPostEditFlowerAsync(CancellationToken ct)
    {
        return this.RunWithToastAsync(
            async () =>
            {
                if (Input.Id == 0) throw new InvalidOperationException("Missing id.");

                var entity = await flowers.GetByIdAsync(Input.Id, ct)
                             ?? throw new KeyNotFoundException("Flower not found.");

                entity.Name = Input.Name.Trim();
                entity.Type = Input.Type.Trim();
                entity.Price = Input.Price;
                entity.CategoryId = Input.CategoryId;
                entity.Sku = string.IsNullOrWhiteSpace(Input.Sku) ? null : Input.Sku.Trim();
                entity.StockQuantity = Input.StockQuantity;
                entity.IsActive = Input.IsActive;
                entity.Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim();

                if (Image is { Length: > 0 })
                    entity.ImagePath = await storage.UploadAsync(Image, MediaFolder.Flowers, ct);
                else if (!string.IsNullOrWhiteSpace(SelectedKey)) entity.ImagePath = SelectedKey;

                await flowers.UpdateAsync(entity, ct);
            },
            "Flower updated.",
            "Update operation failed.",
            "/Flowers/Index"
        );
    }

    public class FlowerEditModel
    {
        public Flower Flower { get; init; } = null!;
        public List<Category> Categories { get; init; } = [];
        public string ModalId => $"flowerEdit-{Flower.Id}";
    }

    public class EditInput
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? Sku { get; set; }
        public int StockQuantity { get; set; } = 0;
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}