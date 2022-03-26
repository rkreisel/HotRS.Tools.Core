namespace HotRS.Tools.Core.Tests;

[ExcludeFromCodeCoverage]

public static class ValidationErrorExtensionsTests
{
	[Test]
	public static void FormatErrorsWithDefaults()
	{
		var source = BuildValidationErrors(3);
		var actual = source.FormatErrors();
		Assert.That(actual, Is.EqualTo("Error 0, Error 1, Error 2"));
	}

	[Test]
	public static void FormatErrorsCustomDelimiter()
	{
		var source = BuildValidationErrors(3);
		var actual = source.FormatErrors(": ");
		Assert.That(actual, Is.EqualTo("Error 0: Error 1: Error 2"));
	}

	[Test]
	public static void FormatErrorsUseLineFeed()
	{
		var source = BuildValidationErrors(3);
		var actual = source.FormatErrors(useLineFeed: true);
		Assert.That(actual, Is.EqualTo("Error 0, \r\nError 1, \r\nError 2"));
	}

	[Test]
	public static void FormatErrorsIncludeMemberNames()
	{
		var source = BuildValidationErrors(3);
		var actual = source.FormatErrors(includeMemberNames: true);
		Assert.That(actual, Is.EqualTo("Error 0 (Member Names: Member0, Member10), Error 1 (Member Names: Member1, Member11), Error 2 (Member Names: Member2, Member12)"));
	}

	private static List<ValidationResult> BuildValidationErrors(int count)
	{
		var result = new List<ValidationResult>();
		for(var ndx = 0; ndx < count; ndx++)
		{
			result.Add(new ValidationResult($"Error {ndx}", new List<string>() { $"Member{ndx}" , $"Member{ndx+10}" }));
		}
		return result;
	}
}
