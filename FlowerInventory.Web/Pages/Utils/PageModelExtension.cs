using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventorySystem.Pages.Utils;

public static class PageModelExtensions
{
    /// <summary>
    ///     Runs an async action, sets TempData toast on success, and a generic error modal on failure.
    ///     Always redirects (PRG).
    /// </summary>
    public static async Task<IActionResult> RunWithToastAsync(
        this PageModel page,
        Func<Task> action,
        string successMessage,
        string errorMessage,
        string redirectPage = "/Index")
    {
        try
        {
            await action();
            page.TempData["SuccessTitle"] = "Success";
            page.TempData["SuccessMessage"] = successMessage;
            page.TempData["msg"] = successMessage;
        }
        catch (OperationCanceledException)
        {
            page.TempData["ErrorTitle"] = "Error";
            page.TempData["ErrorMessage"] = "The operation was canceled.";
        }
        catch
        {
            page.TempData["ErrorTitle"] = "Error";
            page.TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
                ? "Something went wrong. Please try again."
                : errorMessage;
            page.TempData["ErrorId"] = page.HttpContext.TraceIdentifier;
        }

        return page.RedirectToPage(redirectPage);
    }
}