namespace HotRS.Tools.Core.Helpers.Misc;

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
