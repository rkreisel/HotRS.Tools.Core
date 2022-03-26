using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Helpers.HttpRequestHelper
{
	/// <summary>
	/// PatchContent class
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class PatchContent : StringContent
    {
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value"></param>
		public PatchContent(object value)
		   : base(JsonConvert.SerializeObject(value), Encoding.UTF8,
					 "application/json-patch+json")
		{
		}
	}
}
