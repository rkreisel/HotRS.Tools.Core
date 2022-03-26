using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RK.HotRS.ToolsCore.Extensions
{
    /// <summary>
    /// Extension Methods for Exceptions
    /// </summary>
    [ExcludeFromCodeCoverage]
	public static class ExceptionExtensions
    {
		/// <summary>
		/// Adds user defined data (in the form of a key-value pair dictionary) to the exception.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static T SetData<T>(this T source, IDictionary<string, string> data) where T : Exception
		{
			source.CheckForNull(nameof(source));

			if (data != null)
			{
				foreach (var item in data)
				{
					source.Data.Add(item.Key, item.Value);
				}
			}
			return source as T;
		}

		/// <summary>
		/// Sets the HelpLink property of an exception.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="helpLink"></param>
		/// <returns></returns>
		public static T SetHelpLink<T>(this T source, string helpLink) where T : Exception
		{
			source.CheckForNull(nameof(source));
			source.HelpLink = helpLink;
			return source as T;
		}

		/// <summary>
		/// Gets all the exceptions in an array
		/// </summary>
		/// <param name="ex"></param>
		/// <returns>An array of exceptions</returns>
		public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
		{
			if (ex == null)
			{
				throw new ArgumentNullException(nameof(ex));
			}

			var innerException = ex;
			do
			{
				yield return innerException;
				innerException = innerException.InnerException;
			}
			while (innerException != null);
		}

		/// <summary>
		/// Gets all the exception messages in one string
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="withCR"></param>
		/// <returns></returns>
		public static string AllExceptionMessages<T>(this T source, bool withCR = true) where T : Exception
		{
			var msgs = source.GetInnerExceptions().Select(e => e.Message);
			var result = new StringBuilder();
			var ndx = 0;
			var cr = withCR ? Environment.NewLine : "";
			foreach (var msg in msgs)
			{
				result.Append($"{ndx++}: {msg} {cr} ");
			}
			return result.ToString();
		}
	}
}
