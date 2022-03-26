namespace HotRS.Tools.Core.Tests.TestData;

[ExcludeFromCodeCoverage]
public class TestData
{
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public DateTime DateTimeValue { get; set; }
    public TestEnum EnumValue { get; set; }
    public bool BoolValue { get; set; }
    public IList<string> ListValue { get; set; }
}

public enum TestEnum
{
    EnumEntry1 = 1,
    EnumEntry2,
    EnumEntry3,
    EnumEntry4,
    EnumEntry5
}
