using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace RK.HotRS.ToolsCore.Helpers.Misc
{
	[ExcludeFromCodeCoverage]
	public static class ZipTools
	{
		public static IReadOnlyList<ZipArchiveEntry> GetManifest(string fileName)
		{
			var result = new List<ZipArchiveEntry>();
			using (var zf = ZipFile.Open(fileName, ZipArchiveMode.Read))
			{
				result.AddRange(zf.Entries);
			}
			return result;
		}

		public static MemoryStream ExtractFile(string zipFileName, string itemPathAndName)
		{
			using (var result = new MemoryStream())
			{
				using (var zf = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
				{
#pragma warning disable CA1308 // Normalize strings to uppercase
                    var entry = zf.Entries.FirstOrDefault(e => e.Name.ToLower(CultureInfo.InvariantCulture) == itemPathAndName.ToLower(CultureInfo.InvariantCulture));
#pragma warning restore CA1308 // Normalize strings to uppercase
                    if (entry != null)
					{
						using (var str = entry.Open())
						{
							str.CopyTo(result);
							result.Position = 0;
							return result;
						}
					}
					else
					{
						return null;
					}
				}
			}
		}
	}
}
