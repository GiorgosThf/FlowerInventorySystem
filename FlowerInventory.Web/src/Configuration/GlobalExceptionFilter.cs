using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FlowerInventorySystem.FlowerInventory.Web.Configuration;

/* Exception filter for exception handling */
public class GlobalExceptionFilter(
    IHostEnvironment env,
    ITempDataDictionaryFactory tempFactory) : BaseComponent<GlobalExceptionFilter>, IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        /* Log Exception */
        Logger.LogError(context.Exception, "Unhandled exception at {Path}", context.HttpContext.Request.Path);
        
        var accept = context.HttpContext.Request.GetTypedHeaders().Accept;
        var wantsHtml = accept.Count == 0 ||
                        accept.Any(a => a.MediaType.Value!.Contains("html", StringComparison.OrdinalIgnoreCase));

        if (!wantsHtml) return Task.CompletedTask;
        var temp = tempFactory.GetTempData(context.HttpContext);
        /* Standard error title */
        temp["ErrorTitle"] = "Unexpected error";
        /* Set error message based on environment for visuals to be nicer  */
        temp["ErrorMessage"] = env.IsDevelopment()
            ? context.Exception.Message
            : "An unexpected error occurred. Please try again.";
        
        /* Log the error with the id of current activity for debugging */
        temp["ErrorId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

        var back = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString.ToUriComponent();
        context.Result = new RedirectResult(back);
        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}