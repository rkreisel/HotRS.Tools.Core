using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Helpers.HttpRequestHelper
{
	/// <summary>
	/// A JSONContent object
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class JsonContent : StringContent
    {
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value"></param>
		public JsonContent(object value)
		   : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
		{
		}

		/// <summary>
		/// Constructor with media type override
		/// </summary>
		/// <param name="value"></param>
		/// <param name="mediaType"></param>
		public JsonContent(object value, string mediaType)
			: base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
		{
		}
	}
}
