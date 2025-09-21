using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlowerInventory.Web.Pages;

public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    public int StatusCode { get; private set; } = 500;
    public string? Path { get; private set; }
    public string TraceId => Activity.Current?.Id ?? HttpContext.TraceIdentifier;

    public void OnGet(int? code = null)
    {
        StatusCode = code ?? 500;
        Path = HttpContext.Request.Path;
    }

    public void OnGetWithStatusCode(int code)
    {
        OnGet(code);
    }

    public void OnGetError()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        Path = feature?.Path;
        logger.LogError(feature?.Error, "Unhandled exception at {Path}. TraceId={TraceId}", Path, TraceId);
    }
}