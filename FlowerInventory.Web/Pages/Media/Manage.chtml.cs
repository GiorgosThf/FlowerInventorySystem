using FlowerInventory.Web.Dto;
using FlowerInventory.Web.Model;
using FlowerInventory.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages.Media;

public class ManageModel(IFileStorage storage) : PageModel
{
    [BindProperty(SupportsGet = true)] public MediaFolder Folder { get; set; } = MediaFolder.Flowers;

    // Optional client-side filter box exists; we still expose Q to keep URL in sync
    [BindProperty(SupportsGet = true)] public string? Q { get; set; }

    public List<StorageObject> Items { get; private set; } = [];

    [BindProperty] public IFormFile[] Files { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Items = await storage.ListAsync(Folder, ct);
        // (We filter client-side in JS for responsiveness)
    }

    public async Task<IActionResult> OnPostUploadAsync(CancellationToken ct)
    {
        if (!(Files.Length > 0)) return RedirectToPage(new { Folder, Q });
        foreach (var f in Files.Where(x => x.Length > 0))
            await storage.UploadAsync(f, Folder, ct);
        return RedirectToPage(new { Folder, Q });
    }

    public async Task<IActionResult> OnPostDeleteAsync(string key, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(key))
            await storage.DeleteAsync(key, ct);

        return RedirectToPage(new { Folder, Q });
    }
}