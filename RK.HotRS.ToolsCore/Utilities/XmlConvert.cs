using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HotRS.Tools.Core.Utilities
{
	[ExcludeFromCodeCoverage]
	public static class XmlConverter
	{
		public static string SerializeObject<T>(T dataObject)
		{
			if (dataObject == null)
			{
				return string.Empty;
			}
			try
			{
                using StringWriter stringWriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringWriter, dataObject);
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
			}
		}

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
