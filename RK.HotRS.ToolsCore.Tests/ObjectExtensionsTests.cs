using RK.HotRS.ToolsCore.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RK.HotRS.ToolsCore.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Tests
{
	[ExcludeFromCodeCoverage]

	public static class ObjectExtensionsTests
	{
		[Test]
		public static void ToAndFromByteArraySuccess()
		{
			var sourceObj = new TestObject() { Name = "Bill Gates", Birthdate = new DateTime(1955, 10, 28) };

			//Test ToByteArray()
			var actual = sourceObj.ToByteArray();
			Assert.That(actual, Is.Not.Null);
			Assert.That(actual.Length > 0);

			//Test FromBtyeArray()
			var reconstituted = actual.FromByteArray<TestObject>() as TestObject;
			Assert.That(reconstituted.Name == sourceObj.Name);
			Assert.That(reconstituted.Birthdate == sourceObj.Birthdate);
		}

		[Test]
		public static void CheckForNullObjectIsNull()
		{
			Dictionary<string, string> obj = null;
			Assert.That(() => obj.CheckForNull(nameof(obj)),
				Throws.Exception.TypeOf<ArgumentNullException>()
				.With.Message.EqualTo("Value cannot be null. (Parameter 'obj')"));
		}

		[Test]
		public static void CheckForNullObjectIsNullCustomMessage()
		{
			Dictionary<string, string> obj = null;
			var customMessage = "Custom null object message";
			Assert.That(() => obj.CheckForNull<RKToolsException>(nameof(obj), customMessage),
				Throws.Exception.TypeOf<RKToolsException>()
				.With.Message.EqualTo($"{nameof(obj)} - {customMessage}"));
		}

	   [Test]
		public static void CheckForNullObjectIsNotNull()
		{
			Dictionary<string, string> obj = new Dictionary<string, string>();
			try
			{
				obj.CheckForNull(nameof(obj));
				Assert.IsTrue(true);
			}
			catch
            {
				Assert.Fail();
            }
		}
	}

	[Serializable]
	[ExcludeFromCodeCoverage]
	public class TestObject
	{
		public string Name { get; set; }
		public DateTime Birthdate { get; set; }
	}
}
