using RK.HotRS.ToolsCore.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Tests
{
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
			var actual = "something with a | in it |".AddCSVInjectionProtection();
			Assert.That(actual == @"something with a \| in it \|", Is.True);
		}

		[Test]
		public static void AddCSVInjectionTestNoDouble()
		{
			var actual = "something with a \\| in it \\|".AddCSVInjectionProtection();
			Assert.That(actual == @"something with a \| in it \|", Is.True);
		}

		[Test]
		public static void RemoveCSVInjectionTest()
		{
			var actual = @"something with a \| in it".RemoveCSVInjectionProtection();
			Assert.That(actual == @"something with a | in it", Is.True);
		}

		[Test]
		public static void RemoveCSVInjectionTestDoubles()
		{
			var actual = @"something with a \| in it \|".RemoveCSVInjectionProtection();
			Assert.That(actual == @"something with a | in it |", Is.True);
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
	}
}
