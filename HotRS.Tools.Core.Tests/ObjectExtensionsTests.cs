

namespace HotRS.Tools.Core.Tests;

[ExcludeFromCodeCoverage]

public static class ObjectExtensionsTests
{
    [Test]
    public static void CheckForNullObjectIsNull()
    {
        Dictionary<string, string> obj = null;
        Assert.That(() => ArgumentNullException.ThrowIfNull(obj, nameof(obj)),
            Throws.Exception.TypeOf<ArgumentNullException>()
            .With.Message.EqualTo("Value cannot be null. (Parameter 'obj')"));
    }

    [Test]
    public static void CheckForNullObjectIsNullCustomMessage()
    {
        Dictionary<string, string> obj = null;
        var customMessage = "Custom null object message";
        Assert.That(() => obj.CheckForNull<HotRSToolsException>(nameof(obj), customMessage),
            Throws.Exception.TypeOf<HotRSToolsException>()
            .With.Message.EqualTo($"{nameof(obj)} - {customMessage}"));
    }

    [Test]
    public static void CheckForNullObjectIsNotNull()
    {
        Dictionary<string, string> obj = new ();
        try
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
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
