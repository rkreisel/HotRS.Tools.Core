using RK.HotRS.ToolsCore.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Tests
{
	[ExcludeFromCodeCoverage]

	public static class CollectionExtensionsTests
	{
		[Test]
		public static void IsNullOrEmptyIsEmpty()
		{
			var expected = BuildList(0);
			Assert.That(expected.IsNullOrEmpty(), Is.True);
		}

		[Test]
		public static void IsNullOrEmptyIsNull()
		{
			List<string> expected = null;
			Assert.That(expected.IsNullOrEmpty(), Is.True);
		}

		[Test]
		public static void IsNullOrEmptyIsFalse()
		{
			var expected = BuildList(2);
			Assert.That(expected.IsNullOrEmpty(), Is.False);
		}

		private static List<string> BuildList(int count)
		{
			var result = new List<string>();
			for(var ndx = 0; ndx < count; ndx++)
			{
				result.Add($"Item {ndx}");
			}
			return result;
		}
	}	
}
