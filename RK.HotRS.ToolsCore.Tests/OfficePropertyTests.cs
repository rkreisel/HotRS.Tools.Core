using NUnit.Framework;
using RK.HotRS.ToolsCore.Helpers.Office;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace RK.HotRS.ToolsCore.Tests
{
	[ExcludeFromCodeCoverage]

	public class OfficePropertyTests
	{
		[SetUp]
		public void Setup()
		{
		}
		[Test]
		[TestCase("TestDocM.docm")]
		[TestCase("TestDocX.docx")]
		[TestCase("TestXls strict.xlsx")]
		[TestCase("TestXls.xlsx")]
		[TestCase("TestXlsM.xlsm")]
		[TestCase("TestXlsB.xlsb")]
		[TestCase("TestPptX.pptx")]
		[TestCase("TestPptM.pptm")]
		public void GetPropertiesSucceeds(string fn)
		{
			var fileName = $"..\\..\\..\\TestData\\{fn}";
			if (!File.Exists(fileName))
			{
				var msg = $"Could not locate {fileName}";
				Trace.WriteLine(msg);
				throw new FileNotFoundException(msg);
			}
			var sut = new PropertyHelper();
			var sw = new Stopwatch();
			sw.Start();
			var properties = sut.GetProperties(fileName);
			sw.Stop();
			Trace.WriteLine($"Evaluating a {fileName} required {sw.ElapsedMilliseconds} milliseconds.");
			Assert.That(properties != null, Is.True);
			Assert.That(properties.Any(m => m.PropName.ToLower().Contains("status") && m.PropValue == "Complete"), Is.True, "Status wrong");
			Assert.That(properties.Any(m => m.PropName.ToLower().Contains("category") && m.PropValue == "Category Value"), Is.True, "Categories wrong");
			Assert.That(properties.Any(m => m.PropName.ToLower().Contains("subject") && m.PropValue == "Subject Value"), Is.True, "Subject wrong");
			Assert.That(properties.Any(m => m.PropName.ToLower().Contains("hyperlinkbase") && m.PropValue == "HL Base"), Is.True, "Hyperlink Base wrong");
			Assert.That(properties.Any(m => m.PropName.ToLower().Contains("company") && m.PropValue == "Company Value"), Is.True, "Company wrong");
		}

		[Test]
		public void GetPropertiesFailsUnsupportedFile()
		{
			var fileName = @"..\..\..\TestData\TestXls97.xls";
			if (!File.Exists(fileName))
			{
				var msg = $"Could not locate {fileName}";
				Trace.WriteLine(msg);
				throw new FileNotFoundException(msg);
			}
			var sut = new PropertyHelper();
			Assert.That(() => sut.GetProperties(fileName),
				Throws.TypeOf<ApplicationException>()
				.With.Property("Message")
				.Contains($"Could not open {fileName} as a Zip file. Most likely cause is an invalid file (not an Office 2007 or later file)."));
		}

		[Test]
		public void GetPropertiesFailsFileNotFound()
		{
			var sut = new PropertyHelper();
			Assert.That(() => sut.GetProperties("badfilename.xslx"), Throws.Exception.TypeOf<FileNotFoundException>());
		}
	}
}
