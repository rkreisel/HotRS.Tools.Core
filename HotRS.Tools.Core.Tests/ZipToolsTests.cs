﻿namespace HotRS.Tools.Core.Tests;

[ExcludeFromCodeCoverage]

public class ZipToolsTests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void GetManifestSucceeds()
	{
		var manifest = ZipTools.GetManifest(@"..\..\..\TestData\TestDocM.docm");
		Assert.That(manifest != null, Is.True);
	}

	[Test]
	public void GetManifestFailsFileNotFound() =>
		Assert.That(() => ZipTools.GetManifest("badfilename.xslx"), Throws.Exception.TypeOf<FileNotFoundException>());

}
