using RK.HotRS.ToolsCore.Helpers.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace RK.HotRS.ToolsCore.Helpers.Office
{
	[ExcludeFromCodeCoverage]
	public class PropertyHelper
	{
		public IReadOnlyList<OfficeProperty> GetProperties(string fileName)
		{
			IReadOnlyList<ZipArchiveEntry> manifest = null;
			try
			{
				manifest = ZipTools.GetManifest(fileName);
			}
			catch (FileNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ApplicationException($"Could not open {fileName} as a Zip file. Most likely cause is an invalid file (not an Office 2007 or later file). Origianl Error: {ex.Message}");
			}
			var props = new List<OfficeProperty>();
			var nsManager = BuildNSManager();

			foreach (var entry in manifest.Where(e => e.FullName.IndexOf("props", StringComparison.InvariantCultureIgnoreCase) >= 0 && e.Name.Contains(".xml", StringComparison.InvariantCultureIgnoreCase)))
			{
				using (var xmlstream = ZipTools.ExtractFile(fileName, entry.Name))
				{
					var xmlString = Encoding.ASCII.GetString(xmlstream.ToArray());
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(xmlString);
					var xmlSrch = "//*[contains(name(), 'ropert')]";
					var nodes = xmlDoc.SelectNodes(xmlSrch);
					foreach (XmlNode node in nodes)
					{
						props.AddRange(GetChildData(node));
					}
				}
			}
			return props;
		}

		private IList<OfficeProperty> GetChildData(XmlNode node)
		{
			var result = new List<OfficeProperty>();
			if (node.HasChildNodes)
			{
				foreach (XmlNode cn in node.ChildNodes)
				{
					var cds = GetChildData(cn);
					foreach (var cd in cds)
					{
						result.Add(cd);
					}
				}
			}
			else
			{
				result.Add(new OfficeProperty { PropName = node.ParentNode.Name, PropValue = node.Value, PropXMLPath = node.ParentNode.NamespaceURI });
			}
			return result;
		}

		private static XmlNamespaceManager BuildNSManager()
		{
			const string corePropertiesSchema = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
			const string dcPropertiesSchema = "http://purl.org/dc/elements/1.1/";
			const string dcTermsPropertiesSchema = "http://purl.org/dc/terms/";
			const string vtSchema = "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes";
			NameTable nt = new NameTable();
			XmlNamespaceManager nsManager = new XmlNamespaceManager(nt);
			nsManager.AddNamespace("cp", corePropertiesSchema);
			nsManager.AddNamespace("dc", dcPropertiesSchema);
			nsManager.AddNamespace("ds", dcPropertiesSchema);
			nsManager.AddNamespace("dcterms", dcTermsPropertiesSchema);
			nsManager.AddNamespace("vt", vtSchema);
			return nsManager;
		}
	}

}
