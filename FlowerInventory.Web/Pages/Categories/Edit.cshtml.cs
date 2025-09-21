using System.ComponentModel.DataAnnotations;
using FlowerInventory.Web.Model;
using FlowerInventory.Web.Pages.Utils;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Categories;

public class Edit(ICategoryService categories, IFileStorage storage) : PageModel
{
    [BindProperty] public EditInput Input { get; set; } = new();
    [BindProperty] public IFormFile? Image { get; set; }
    [BindProperty] public string? SelectedKey { get; set; }


    public Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        return this.RunWithToastAsync(
            async () =>
            {
                if (!ModelState.IsValid)
                    throw new ValidationException("Invalid category data.");

                var entity = await categories.GetByIdAsync(Input.Id, ct)
                             ?? throw new KeyNotFoundException("Category not found.");

                entity.Name = Input.Name.Trim();
                entity.Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim();
                entity.IsActive = Input.IsActive;
                entity.DisplayOrder = Input.DisplayOrder;

                if (Image is { Length: > 0 })
                    entity.ImagePath = await storage.UploadAsync(Image, MediaFolder.Categories, ct);
                else if (!string.IsNullOrWhiteSpace(SelectedKey)) entity.ImagePath = SelectedKey;

                await categories.UpdateAsync(entity, ct);
            },
            "Category updated.",
            "Update category failed.",
            "/Categories/Index"
        );
    }

    public class EditInput
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }

        [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

        [StringLength(400)] public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, int.MaxValue)] public int DisplayOrder { get; set; } = 0;
    }
}