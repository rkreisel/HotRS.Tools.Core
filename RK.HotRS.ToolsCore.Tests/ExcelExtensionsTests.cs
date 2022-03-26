using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;
using OfficeOpenXml;
using RK.HotRS.ToolsCore.Exceptions;
using RK.HotRS.ToolsCore.Extensions;

namespace RK.HotRS.ToolsCore.Tests
{
    [ExcludeFromCodeCoverage]

    public static class ExcelExtensionsTests
	{
        [Test]
        public static void GetNamedRangeValueXlsmSuccess()
        {
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            var actual = package.Workbook.GetNamedRange("TestRange");
            Assert.That(actual, Is.TypeOf<ExcelNamedRange>());
        }

        [Test]
        public static void GetNamedRangeValueXlsmSilentFailure()
        {
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            var actual = package.Workbook.GetNamedRange("MissingRange");
            Assert.That(actual, Is.Null);
        }

        [Test]
        public static void SetNamedRangeValueXlsmSuccess()
        {
            var rangeName = "TestRange";
            var newValue = "TestValue";
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            var range = package.Workbook.GetNamedRange(rangeName);
            Assert.That(range, Is.TypeOf<ExcelNamedRange>());
            Assert.IsTrue(range.Value.ToString() != newValue.ToString());

            var actual = package.Workbook.SetNamedRangeValue(rangeName, newValue);
            Assert.IsTrue(actual);
        }

        [Test]
        public static void SetNamedRangeValueXlsmFailureWithException()
        {
            var rangeName = "MissingRange";
            var newValue = "newvalue";
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            Assert.That(() => package.Workbook.SetNamedRangeValue(rangeName, newValue, true),
                Throws.Exception.TypeOf<KeyNotFoundException>()
                .With.Message.EqualTo($"The given key '{rangeName}' was not present in the dictionary."));
        }

        [Test]
        public static void AddNamedRangeFailRangeExists()
        {
            var rangeName = "TestRange";
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            var workBook = package.Workbook;
            Assert.That(() => workBook.AddNamedRange(null, rangeName, null),
                Throws.Exception.TypeOf<RKToolsException>()
                .With.Message.Contains("Requested Named Range already exists at"));
        }

        [Test]
        public static void AddNamedRangeSuccessWithSheetNamed()
        {
            var rangeName = "NewRange";
            var newValue = "newvalue";
            var sheetName = "Sheet1";
            var targetRange = "$B$6";
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            var workBook = package.Workbook;
            var sheet = workBook.Worksheets[0];
            workBook.AddNamedRange(sheetName, rangeName, targetRange, newValue);
            var actual = workBook.GetNamedRange(rangeName);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(newValue));
        }

        [TestCase(eWorkSheetHidden.Visible)]
        [TestCase(eWorkSheetHidden.Hidden)]
        [TestCase(eWorkSheetHidden.VeryHidden)]
        public static void AddNamedRangeSuccessGlobalHiddenStateTests(eWorkSheetHidden state)
        {
            var rangeName = "NewRange";
            var newValue = "newvalue";
            string sheetName = "insertedSheet";
            var targetRange = "$B$6";
            var fi = new FileInfo(@".\testdata\TestXlsMWithMacro.xlsm");
            using var package = new ExcelPackage(fi);
            var workBook = package.Workbook;
            var sheet = workBook.Worksheets[0];
            workBook.AddNamedRange(sheetName, rangeName, targetRange, newValue, true, state);
            var actual = workBook.GetNamedRange(rangeName);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Value, Is.EqualTo(newValue));
            Assert.That(workBook.Worksheets[sheetName], Is.Not.Null);
            Assert.That(workBook.Worksheets[sheetName].Hidden, Is.EqualTo(state));
        }
    }
}
