namespace HotRS.Tools.Core.Helpers.HttpRequestHelper;

/// <summary>
/// Extensions for the HTTPResponse class
/// </summary>
[ExcludeFromCodeCoverage]
public static class HttpResponseExtensions
{
	/// <summary>
	/// Convert content into the desired type
	/// </summary>
	/// <typeparam name="T">The desired type</typeparam>
	/// <param name="response"></param>
	/// <returns>An object of the desired type</returns>
	public async static Task<T> ContentAsType<T>(this HttpResponseMessage response)
	{
		if (response ==  null)
		{
			throw new ArgumentNullException(nameof(response));
		}

		var settings = new JsonSerializerSettings
		{
			MissingMemberHandling = MissingMemberHandling.Ignore
		};
		var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		return string.IsNullOrWhiteSpace(data)
			? default
			: JsonConvert.DeserializeObject<T>(data, settings);
	}

	/// <summary>
	/// Converts the content into JSON
	/// </summary>
	/// <param name="response"></param>
	/// <returns>A JSON string</returns>
	public async static Task<string> ContentAsJson(this HttpResponseMessage response) =>
		JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false) ?? default);

	/// <summary>
	/// Converts the content into a string
	/// </summary>
	/// <param name="response"></param>
	/// <returns>A string</returns>
	public async static Task<string> ContentAsString(this HttpResponseMessage response) =>
		await response.Content.ReadAsStringAsync().ConfigureAwait(false);

	/// <summary>
	/// Sets the common properties of the class
	/// </summary>
	/// <param name="source"></param>
	/// <param name="acceptHeader"></param>
	/// <param name="bearerToken"></param>
	/// <param name="cancellationToken"></param>
	/// <param name="completionOption"></param>
	/// <param name="timeout"></param>
	/// <param name="maxResponseSize"></param>
	/// <returns>An the provided HTTPBuilder object with all the properties set</returns>
	public static HttpRequestBuilder SetCommonProperties(
		this HttpRequestBuilder source,
		string acceptHeader = null,
		string bearerToken = null,
		CancellationToken? cancellationToken = null,
		HttpCompletionOption? completionOption = null,
		TimeSpan? timeout = null,
		long? maxResponseSize = null)
	{
		source.CheckForNull<ArgumentNullException>(nameof(source), Resources.ARGUEMENTNOTPROVIDED);
		source
			.SetAcceptHeader(acceptHeader)
			.SetBearerToken(bearerToken)
			.SetCancellationToken(cancellationToken)
			.SetHttpCompletionOption(completionOption)
			.SetTimeout(timeout)
			.SetMaxResponseSize(maxResponseSize);

		return source;
	}

}
