namespace HotRS.Tools.Core.Utilities
{
	/// <summary>
	/// Simplifies converting xml objects to and from a string
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class XmlConverter
	{
		/// <summary>
		/// Convert and XML object to a string
		/// </summary>
		/// <typeparam name="T">The type of the object to convert</typeparam>
		/// <param name="dataObject">The object</param>
		/// <returns>A string version of the XML  object</returns>
		public static string SerializeObject<T>(T dataObject)
		{
			if (dataObject == null)
			{
				return string.Empty;
			}
			try
			{
                using StringWriter stringWriter = new();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringWriter, dataObject);
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
			}
		}

		/// <summary>
		/// Convert a stirng version of XML into an XML object
		/// </summary>
		/// <typeparam name="T">The target type</typeparam>
		/// <param name="xml">The string version of he XML</param>
		/// <returns>An XML object</returns>
		public static T DeserializeObject<T>(string xml)
			 where T : new()
		{
			if (string.IsNullOrEmpty(xml))
			{
				return new T();
			}
			try
			{
                //using (var stringReader = new StringReader(xml))
                using var stringReader = XmlReader.Create(xml);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                return new T();
			}
		}
	}
}
