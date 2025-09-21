using FlowerInventorySystem.FlowerInventory.Web.Model;
using FlowerInventorySystem.FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventorySystem.Pages.Flowers;

public class IndexModel(IFlowerService flowers, ICategoryService categories) : PageModel
{
    public List<Flower> Items { get; private set; } = [];
    public List<Category> CategoryList { get; private set; } = [];

    [BindProperty(SupportsGet = true)] public string? Q { get; set; }
    [BindProperty(SupportsGet = true)] public int? CategoryId { get; set; }
    [BindProperty(SupportsGet = true)] public string? Sort { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Items = await flowers.SearchAsync(Q, CategoryId, Sort, ct);
        CategoryList = await categories.GetAllAsync(ct);
    }

    public string Toggle(string col)
    {
        return Sort == col ? col + "_desc" : col;
    }
}