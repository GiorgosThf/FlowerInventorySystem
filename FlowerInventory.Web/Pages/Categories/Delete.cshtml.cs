using FlowerInventory.Web.Pages.Utils;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Categories;

public class Delete(ICategoryService categories) : PageModel
{
    public Task<IActionResult> OnPostAsync(int id, CancellationToken ct)
    {
        return this.RunWithToastAsync(
            async () =>
            {
                var f = await categories.GetByIdAsync(id, ct);

                if (f is null) throw new InvalidOperationException("Not found");

                await categories.DeleteAsync(f, ct);
            },
            $"Category with id {id} has been deleted.",
            $"Error deleting category {id}.",
            "/Categories/Index"
        );
    }
}