using FlowerInventorySystem.FlowerInventory.Web.Model;
using FlowerInventorySystem.FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventorySystem.Pages.Categories;

public class IndexModel(ICategoryService categories) : PageModel
{
    public List<Category> Items { get; private set; } = [];

    [BindProperty(SupportsGet = true)] public string? Q { get; set; }
    [BindProperty(SupportsGet = true)] public string? Sort { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Items = await categories.SearchAsync(Q, Sort, ct);
    }

    public string Toggle(string col)
    {
        return Sort == col ? col + "_desc" : col;
    }
}