using Newtonsoft.Json;
using NUnit.Framework;
using RK.HotRS.ToolsCore.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace RK.HotRS.ToolsCore.Tests
{
    [ExcludeFromCodeCoverage]

    public static class AssemblyExtensionsTests
	{
        [Test]
        public static void GetTextFileFromAssemblySuccess()
        {
            var fileName = "TestData.TestData.json";
            var actual = Assembly.GetExecutingAssembly().GetTextFileFromAssembly(fileName);
            Assert.That(actual.Length, Is.GreaterThan(0));

            var convertedActual = JsonConvert.DeserializeObject<TestData.TestData>(actual);
            Assert.That(convertedActual.BoolValue, Is.EqualTo(true));
            Assert.That(convertedActual.EnumValue, Is.EqualTo(TestData.TestEnum.EnumEntry3));
            Assert.That(convertedActual.IntValue, Is.GreaterThan(-1));
            Assert.That(convertedActual.ListValue.Count, Is.GreaterThan(1));
            Assert.That(convertedActual.StringValue, Is.Not.Empty);
        }

        [Test]
        public static void GetTextFileFromAssemblyNotFound()
        {
            var fileName = "invalidfilename";

            Assert.That(() => Assembly.GetExecutingAssembly().GetTextFileFromAssembly(fileName),
                Throws.Exception.TypeOf<FileNotFoundException>()
                .With.Property("Message").
                EqualTo($"{fileName} does not exist in the assembly."));
        }

    }
}