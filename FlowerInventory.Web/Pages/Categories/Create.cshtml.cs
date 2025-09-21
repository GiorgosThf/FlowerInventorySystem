using System.ComponentModel.DataAnnotations;
using FlowerInventory.Web.Model;
using FlowerInventory.Web.Pages.Utils;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Categories;

public class CreateModel(ICategoryService categories, IFileStorage storage) : PageModel
{
    [BindProperty] public IFormFile? Image { get; set; }
    [BindProperty] public string? SelectedKey { get; set; }
    [BindProperty] public CreateInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        return this.RunWithToastAsync(
            async () =>
            {
                if (!ModelState.IsValid)
                    throw new ValidationException("Invalid category data.");

                var objectKey =
                    Image is { Length: > 0 } ? await storage.UploadAsync(Image, MediaFolder.Categories, ct)
                    : !string.IsNullOrWhiteSpace(SelectedKey) ? SelectedKey!
                    : "default/default-category.jpg";

                var entity = new Category
                {
                    Name = Input.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim(),
                    IsActive = Input.IsActive,
                    DisplayOrder = Input.DisplayOrder,
                    ImagePath = objectKey
                };

                await categories.CreateAsync(entity, ct);
            },
            "Category created.",
            "Create category operation failed.",
            "/Categories/Index"
        );
    }

    public class CreateInput
    {
        [Required] [StringLength(100)] public string Name { get; init; } = string.Empty;

        [StringLength(400)] public string? Description { get; init; }

        public bool IsActive { get; init; } = true;

        [Range(0, int.MaxValue)] public int DisplayOrder { get; init; }
    }
}