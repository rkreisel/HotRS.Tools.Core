using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RK.HotRS.ToolsCore.Extensions
{
	/// <summary>
	/// Extensions to Object
	/// </summary>
	public static class ObjectExtensions
    {
		/// <summary>
		/// Converts an object to a byte array.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(this object source)
		{
			if (source == null)
			{
				return null;
			}
			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, source);
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Convert a byte array back to an object of type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public static T FromByteArray<T>(this byte[] data)
		{
			if (data == null)
				return default;
			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream(data))
			{
				var obj = bf.Deserialize(ms);
				return (T)obj;
			}
		}

		public static void CheckForNull(this object o, string paramName)
		{
			if (o == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}
		public static void CheckForNull<T>(this object o, string paramName, string message = "") where T : Exception
		{
			if (o == null)
			{
				var formattedMessage = string.IsNullOrWhiteSpace(message) ? paramName : $"{paramName} - {message}";
				throw (T)Activator.CreateInstance(typeof(T), new object[] { $"{formattedMessage}" });
			}
		}
	}
}
