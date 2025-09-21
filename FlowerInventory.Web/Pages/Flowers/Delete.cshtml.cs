using FlowerInventory.Web.Pages.Utils;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Flowers;

public class DeleteModel(IFlowerService flowers) : PageModel
{
    public Task<IActionResult> OnPostAsync(int id, CancellationToken ct)
    {
        return this.RunWithToastAsync(
            async () =>
            {
                var f = await flowers.GetByIdAsync(id, ct);

                if (f is null) throw new InvalidOperationException("Not found");

                await flowers.DeleteAsync(f, ct);
            },
            "Deleted.",
            "Delete operation failed.",
            "/Flowers/Index"
        );
    }
}