using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FlowerInventory.Web.Model;
using FlowerInventory.Web.Pages.Utils;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Flowers;

public class CreateModel(IFlowerService flowers, ICategoryService categories, IFileStorage storage) : PageModel
{
    [BindProperty] public IFormFile? Image { get; set; }
    [BindProperty] public string? SelectedKey { get; set; }
    [BindProperty] public CreateInput Input { get; set; } = new();

    public List<Category> CategoryList { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        CategoryList = await categories.GetAllAsync(ct);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (ModelState.IsValid)
            return await this.RunWithToastAsync(
                async () =>
                {
                    var objectKey =
                        Image is { Length: > 0 } ? await storage.UploadAsync(Image, MediaFolder.Flowers, ct)
                        : !string.IsNullOrWhiteSpace(SelectedKey) ? SelectedKey!
                        : "default/default-flower.jpg";

                    var entity = new Flower
                    {
                        PublicId = Guid.NewGuid(),
                        Name = Input.Name.Trim(),
                        Type = Input.Type.Trim(),
                        Price = Input.Price,
                        CategoryId = Input.CategoryId,
                        Sku = string.IsNullOrWhiteSpace(Input.Sku) ? null : Input.Sku.Trim(),
                        StockQuantity = Input.StockQuantity,
                        IsActive = Input.IsActive,
                        Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim(),
                        ImagePath = objectKey
                    };

                    await flowers.CreateAsync(entity, ct);
                },
                "Flower created.",
                "Create operation failed.",
                "/Flowers/Index"
            );
        CategoryList = await categories.GetAllAsync(ct);
        return Page();
    }

    public class CreateInput
    {
        [Required] [StringLength(120)] public string Name { get; init; } = string.Empty;

        [Required] [StringLength(80)] public string Type { get; init; } = string.Empty;

        [Range(0.01, 999999)] public decimal Price { get; init; }

        [Required] [DisplayName("Category")] public int CategoryId { get; init; }

        [StringLength(60)] public string? Sku { get; init; }

        [Range(0, int.MaxValue)]
        [DisplayName("Stock quantity")]
        public int StockQuantity { get; init; }

        [DisplayName("Active")] public bool IsActive { get; init; } = true;

        [StringLength(1000)] public string? Description { get; init; }
    }
}