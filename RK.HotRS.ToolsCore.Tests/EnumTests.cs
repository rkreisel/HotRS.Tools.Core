using RK.HotRS.ToolsCore.Extensions;
using NUnit.Framework;
using System.Linq;
using static RK.HotRS.ToolsCore.Extensions.EnumExtensions;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Tests
{

	enum TestEnum : short
	{
		[System.ComponentModel.Description("This is the description for Value1")]
		Value1 = 1,
		[System.ComponentModel.Description("This is the description for Value2")]
		Value2 = 2,
		Value3 = 3,
		[System.ComponentModel.DataAnnotations.DataType("Text")]
		Value4 = 4,
		Value5 = 5,
		Value10 = 10,
		Value11 = 11
	}

	[ExcludeFromCodeCoverage]
	public static class EnumExtensionsTests
	{
		[Test]
		public static void GetEnumDescriptionTestPresent() => 
			Assert.That("This is the description for Value1", Is.EqualTo(TestEnum.Value1.GetEnumDescription()));

		[Test]
		public static void GetEnumDescriptionTestMissing() => 
			Assert.That("Value3", Is.EqualTo(TestEnum.Value3.GetEnumDescription()));

		[Test]
		public static void GetValueTestWithDescription() => 
			Assert.AreEqual(TestEnum.Value2, TestEnum.Value2.GetValueFromDescription("This is the description for Value2"));

		[Test]
		public static void GetValueTestWithoutDescription() => 
			Assert.That(TestEnum.Value3.GetValueFromDescription(""), Is.Null);

		[Test]
		public static void AsListUsingDescriptions()
		{
			var e = new Enum<TestEnum>().AsList(true);
			Assert.That(e.Count == 7);
			Assert.That(e.Any(i => i.ToLower() == "this is the description for value1"), Is.True);
			Assert.That(e.Any(i => i.ToLower() == "value5"), Is.True);
		}

		[Test]
		public static void AsListNotUsingDescriptions()
		{
			var e = new Enum<TestEnum>().AsList();
			Assert.That(e.Count == 7);
			Assert.That(e.Any(i => i.ToLower() == "this is the description for value1"), Is.False);
			Assert.That(e.Any(i => i.ToLower() == "value5"), Is.True);
		}
	}
}