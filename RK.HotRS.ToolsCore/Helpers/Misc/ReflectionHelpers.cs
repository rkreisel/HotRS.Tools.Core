using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RK.HotRS.ToolsCore.Helpers.Misc
{
    /// <summary>
    /// Miscellaneous Reflection helpers and extensions
    /// </summary>
    [ExcludeFromCodeCoverage]
	public static class ReflectionHelpers
    {
		/// <summary>
		/// Returns the executing method name
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetCurrentMethod()
		{
			var st = new StackTrace();
			var sf = st.GetFrame(1);
			return sf.GetMethod().Name;
		}
	}
}
