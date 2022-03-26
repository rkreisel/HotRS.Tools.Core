namespace HotRS.Tools.Core.Helpers.Misc;

/// <summary>
/// Tools for zip files
/// </summary>
[ExcludeFromCodeCoverage]
public static class ZipTools
{
	/// <summary>
    /// Read the zip file manifest
    /// </summary>
    /// <param name="fileName">The zip file name</param>
    /// <returns>A list of items in the zip file</returns>
    public static IReadOnlyList<ZipArchiveEntry> GetManifest(string fileName)
	{
		var result = new List<ZipArchiveEntry>();
		using (var zf = ZipFile.Open(fileName, ZipArchiveMode.Read))
		{
			result.AddRange(zf.Entries);
		}
		return result;
	}

	/// <summary>
    /// Extract a single file from a Zip file.
    /// </summary>
    /// <param name="zipFileName">Teh zip File Name</param>
    /// <param name="itemPathAndName">The full path to the desired file</param>
    /// <returns>A memory stream which can be read to export the file.</returns>
    public static MemoryStream ExtractFile(string zipFileName, string itemPathAndName)
	{
        using var result = new MemoryStream();
        using var zf = ZipFile.Open(zipFileName, ZipArchiveMode.Read);
        var entry = zf.Entries.FirstOrDefault(e => e.Name.ToLower(CultureInfo.InvariantCulture) == itemPathAndName.ToLower(CultureInfo.InvariantCulture));
        if (entry != null)
        {
            using var str = entry.Open();
            str.CopyTo(result);
            result.Position = 0;
            return result;
        }
        else
        {
            return null;
        }
    }
}
