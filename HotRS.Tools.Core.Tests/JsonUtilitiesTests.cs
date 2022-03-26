namespace HotRS.Tools.Core.Tests;

[ExcludeFromCodeCoverage]

public static class JsonUtilitiesTests
{
    [Test]
    public static void IgnorePreferredNamePropertyTest()
    {
        var sourceObj = new ComplexTestObject()
        {
            Name = "William Gates",
            Birthdate = new DateTime(1955, 10, 28),
            PreferredName = "Bill",
            ChildObj = new ChildObject { Key = 1, Value = "One" }
        };

        var jsonResolver = new PropertyRenameOrIgnoreSerializerContractResolver();
        jsonResolver.IgnoreProperty(typeof(ComplexTestObject), "PreferredName");
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = jsonResolver
        };

        var actual = JsonConvert.SerializeObject(sourceObj, serializerSettings);
        Assert.That(actual.Contains("PreferredName"), Is.False);
    }

    [Test]
    public static void IgnorePropertyInChildTest()
    {
        var sourceObj = new ComplexTestObject()
        {
            Name = "William Gates",
            Birthdate = new DateTime(1955, 10, 28),
            PreferredName = "Bill",
            ChildObj = new ChildObject { Key = 1, Value = "One" }
        };

        var jsonResolver = new PropertyRenameOrIgnoreSerializerContractResolver();
        jsonResolver.IgnoreProperty(typeof(ChildObject), "Key");
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = jsonResolver
        };

        var actual = JsonConvert.SerializeObject(sourceObj, serializerSettings);
        Assert.That(actual.Contains("Key"), Is.False);
    }

    [Test]
    public static void IgnoreMultiplePropertiesTest()
    {
        var sourceObj = new ComplexTestObject()
        {
            Name = "William Gates",
            Birthdate = new DateTime(1955, 10, 28),
            PreferredName = "Bill",
            ChildObj = new ChildObject { Key = 1, Value = "One" }
        };

        var jsonResolver = new PropertyRenameOrIgnoreSerializerContractResolver();
        jsonResolver.IgnoreProperty(typeof(ComplexTestObject), "PreferredName");
        jsonResolver.IgnoreProperty(typeof(ChildObject), "Key");
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = jsonResolver
        };

        var actual = JsonConvert.SerializeObject(sourceObj, serializerSettings);
        Assert.That(actual.Contains("PreferredName"), Is.False);
        Assert.That(actual.Contains("Key"), Is.False);
    }

    [Serializable]
    public class ComplexTestObject
    {
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string PreferredName { get; set; }
        public ChildObject ChildObj { get; set; }
    }

    [Serializable]
    public class ChildObject
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
