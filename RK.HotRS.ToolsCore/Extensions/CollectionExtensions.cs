namespace HotRS.Tools.Core.Extensions;

/// <summary>
/// Provides custom extensions to a <see cref="List{T}">generic List</see>
/// </summary>
public static class CollectionExtensions
    {
	/// <summary>
	/// Returns true true if the List is empty or null.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <returns>True or False</returns>
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => (source == null || !source.Any());
}

