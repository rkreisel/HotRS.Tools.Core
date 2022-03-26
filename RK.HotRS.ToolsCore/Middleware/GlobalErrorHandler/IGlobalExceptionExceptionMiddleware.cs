namespace HotRS.Tools.Core.Middleware.GlobalErrorHandler;

/// <summary>
/// Provides a Global handler for uncaught exceptions.
/// This was created for .net 3.0 and has probably been obsoleted by now.
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
