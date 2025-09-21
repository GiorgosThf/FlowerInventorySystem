using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventorySystem.Pages;

public class PrivacyModel(ILogger<PrivacyModel> logger) : PageModel
{
    public void OnGet()
    {
        logger.LogInformation("OnGet");
    }
}