using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Helpers.Office
{
	/// <summary>
	/// Class to hold Office Property values extracted from the xml
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class OfficeProperty
	{
		public string PropName { get; set; }
		public string PropXMLPath { get; set; }
		public string PropValue { get; set; }
		public string PropFilePath { get; set; }
		public string PropFileName { get; set; }
	}
}
