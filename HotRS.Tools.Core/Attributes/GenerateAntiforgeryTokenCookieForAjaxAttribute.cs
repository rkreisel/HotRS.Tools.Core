
namespace HotRS.Tools.Core.Attributes;

/// <summary>
/// Defines an AnitForgeryToken cookie attribute
/// </summary>
[AttributeUsage(AttributeTargets.All)]
[ExcludeFromCodeCoverage]
public sealed class GenerateAntiforgeryTokenCookieForAjaxAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Overrides the OnActionExecuted method to generate an anti forgery token
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();

        // We can send the request token as a JavaScript-readable cookie, 
        // and Angular will use it by default.
        var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);
        context.HttpContext.Response.Cookies.Append(
            "XSRF-TOKEN",
            tokens.RequestToken,
            new CookieOptions() { HttpOnly = false });
    }
}
