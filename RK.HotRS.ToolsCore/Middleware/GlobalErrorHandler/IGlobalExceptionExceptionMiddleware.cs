using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RK.HotRS.ToolsCore.Middleware.GlobalErrorHandler
{
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
}