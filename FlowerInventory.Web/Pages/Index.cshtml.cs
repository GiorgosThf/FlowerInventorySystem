using FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlowerInventorySystem.Pages;

public class IndexModel(AppDbContext db) : PageModel
{
    public int CategoryCount { get; private set; }
    public int FlowerCount { get; private set; }

    public async Task OnGet(CancellationToken ct)
    {
        CategoryCount = await db.Categories.CountAsync(ct);
        FlowerCount = await db.Flowers.CountAsync(ct);
    }
}