namespace HotRS.Tools.Core.Middleware.GlobalErrorHandler;

/// <summary>
/// 
/// </summary>
public interface IGlobalExceptionExceptionMiddleware
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	Task Invoke(HttpContext context);
}
