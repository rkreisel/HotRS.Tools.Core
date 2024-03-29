﻿namespace HotRS.Tools.Core.Tests;

[ExcludeFromCodeCoverage]

public static class StringExtensionsTests
{
    [Test]
    public static void ToNullableDateTimeTestReturnNull()
    {
        var actual = ((string)null).ToNullableDateTime();
        Assert.That(actual, Is.Null);
    }

    [Test]
    public static void ToNullableDateTimeTestReturnDate()
    {
        var actual = "2017-11-10 11:59:59".ToNullableDateTime();
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.GetType().IsAssignableFrom(typeof(DateTime)), Is.True);
    }

    [Test]
    public static void AddCSVInjectionTest()
    {
        var actual = "something with a | in it |".CSVInjectionProtection(StringExtensions.CSVInjectionProtectionAction.Protect);
        Assert.That(actual == @"something with a \| in it \|", Is.True);
    }

    [Test]
    public static void AddCSVInjectionTestNoDouble()
    {
        var actual = "something with a \\| in it \\|".CSVInjectionProtection(StringExtensions.CSVInjectionProtectionAction.Protect);
        Assert.That(actual == @"something with a \| in it \|", Is.True);
    }

    [Test]
    public static void AddCSVInjectionTestNoSingleOrDouble()
    {
        var actual = "something with a | and a \\ in it".CSVInjectionProtection(StringExtensions.CSVInjectionProtectionAction.Protect);
        Assert.That(actual == @"something with a \| and a \ in it", Is.True);
    }

    [Test]
    public static void AddCSVInjectionTestNull()
    {
        string input = null;
        var actual = input.CSVInjectionProtection(StringExtensions.CSVInjectionProtectionAction.Protect);
        Assert.IsNull(actual);
    }

    [Test]
    public static void RemoveCSVInjectionTest()
    {
        var actual = @"something with a \| in it".CSVInjectionProtection(StringExtensions.CSVInjectionProtectionAction.Clear);
        Assert.That(actual == @"something with a | in it", Is.True);
    }

    [Test]
    public static void RemoveCSVInjectionTestDoubles()
    {
        var actual = @"something with a \| in it \|".CSVInjectionProtection(StringExtensions.CSVInjectionProtectionAction.Clear);
        Assert.That(actual == @"something with a | in it |", Is.True);
    }

    [Test]
    public static void DateStringFromExcelDateStringNull()
    {
        string sourceDate = null;
        var actual = sourceDate.DateStringFromExcelDateString();
        Assert.IsNull (actual);
    }

    [Test]
    public static void DateStringFromExcelDateStringNormalStringDefaultPattern()
    {
        var targetPattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        var sourceDate = DateTime.Now;
        var actual = sourceDate.ToLongDateString().DateStringFromExcelDateString();
        Assert.That(actual == sourceDate.ToString(targetPattern), Is.True);
    }

    [Test]
    public static void DateStringFromExcelDateStringNormalStringCustomPattern()
    {
        var targetPattern = "MMM/dd/yyyy";
        var sourceDate = DateTime.Now;
        var actual = sourceDate.ToLongDateString().DateStringFromExcelDateString(targetPattern);
        Assert.That(actual == sourceDate.ToString(targetPattern), Is.True);
    }
    [Test]
    public static void DateStringFromExcelDateStringMinimumDateReturnsNull()
    {
        var targetPattern = "MMM/dd/yyyy";
        var sourceDate = "01/01/0001";
        var actual = sourceDate.DateStringFromExcelDateString(targetPattern);
        Assert.IsNull(actual);
    }

    [Test]
    public static void DateStringFromExcelDateStringOADateStringDefaultPattern()
    {
        var targetPattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        var testDate = new DateTime(2019, 5, 26);
        // Note: 43611 is the Excel representation of May 26th, 2019
        var actual = "43611".DateStringFromExcelDateString();
        Assert.That(actual == testDate.ToString(targetPattern), Is.True);
    }

    /// <summary>
    /// This method assumes the input is a string representation of a double representing a date as they are understood by Excel
    /// </summary>
    [Test]
    public static void DateStringFromExcelDateStringOADateStringCustomPattern()
    {
        var targetPattern = "dd MMM yyyy";
        var testDate = new DateTime(2019, 5, 26);
        // Note: 43611 is the Excel representation of May 26th, 2019
        var actual = "43611".DateStringFromExcelDateString(targetPattern);
        Assert.That(actual == testDate.ToString(targetPattern), Is.True);
    }

    /// <summary>
    /// This method assumes the input is a string representation of a double representing a date as they are understood by Excel
    /// </summary>
    [Test]
    public static void DateStringFromExcelDateStringExceptionNotConvertable()
    {
        var targetPattern = "MMM/dd/yyyy";
        var sourceDate = DateTime.Now;
        // Note: 43611 is the Excel representation of May 26th, 2019
        Assert.Throws<FormatException>(() => "43362x".DateStringFromExcelDateString(targetPattern));
    }

    [Test]
    public static void AppendListToStringNoPrefix()
    {
        string sourceString = string.Empty;
        var elementList = new List<string>()
        {
            "Element 1",
            "Element 2",
            "Element 3"
        };
        var actual = sourceString.AppendListToString(elementList, null);
        Assert.That(actual, Is.EqualTo("Element 1Element 2Element 3"));
    }

    [Test]
    public static void AppendListToStringNullListReturnsSource()
    {
        string sourceString = "Beginning String";
        List<string> elementList = null;
        var actual = sourceString.AppendListToString(elementList, null);
        Assert.That(actual, Is.EqualTo(sourceString));
    }

    [Test]
    public static void AppendListToStringEmptyListReturnsSource()
    {
        string sourceString = "Beginning String";
        var elementList = new List<string>();
        var actual = sourceString.AppendListToString(elementList, null);
        Assert.That(actual, Is.EqualTo(sourceString));
    }

    [Test]
    public static void AppendListToStringNoBaseSuccess()
    {
        var sourceString = string.Empty;
        var elementList = new List<string>()
        {
            "Element 1",
            "Element 2",
            "Element 3"
        };
        var actual = sourceString.AppendListToString(elementList);
        Assert.That(actual, Is.EqualTo("Element 1, Element 2, Element 3"));
    }

    [Test]
    public static void AppendListToStringNoPrefixSuccess()
    {
        var sourceString = "Beginning ";
        var elementList = new List<string>()
        {
            "Element 1",
            "Element 2",
            "Element 3"
        };
        var actual = sourceString.AppendListToString(elementList);
        Assert.That(actual, Is.EqualTo("Beginning Element 1, Element 2, Element 3"));
    }

    [Test]
    public static void AppendListToStringWithPrefixSuccess()
    {
        var sourceString = "Beginning ";
        var elementList = new List<string>()
        {
            "Element 1",
            "Element 2",
            "Element 3"
        };
        var actual = sourceString.AppendListToString(elementList, ": ");
        Assert.That(actual, Is.EqualTo("Beginning Element 1: Element 2: Element 3"));
    }
    [TestCase("1", ExpectedResult = 1)]
    [TestCase(" ", ExpectedResult = null)]
    [TestCase(null, ExpectedResult = null)]
    public static int? ToNullableIntTests(string input)
    {
        return input.ToNullableInt();
    }
}
