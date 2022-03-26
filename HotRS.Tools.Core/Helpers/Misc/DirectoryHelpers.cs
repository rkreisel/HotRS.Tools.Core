namespace HotRS.Tools.Core.Helpers.Misc;

/// <summary>
/// Helpers for managing directories
/// </summary>
[ExcludeFromCodeCoverage]
public static class DirectoryHelpers
{
    /// <summary>
    /// Deletes files in folder older than X hours
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="hours"></param>
    public static void CleanUp(string folder, int hours)
    {
        if (string.IsNullOrWhiteSpace(folder))
        {
            throw new ArgumentNullException(nameof(folder));
        }

        Directory.GetFiles(folder)
                .Select(f => new FileInfo(f))
                .Where(f => f.LastAccessTimeUtc < DateTime.UtcNow.AddHours(hours))
                .ToList()
                .ForEach(f => f.Delete());
    }

    /// <summary>
    /// Checks for and, if necessary, creates a folder.
    /// </summary>
    /// <param name="path"></param>
    public static void EnsurePathExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
