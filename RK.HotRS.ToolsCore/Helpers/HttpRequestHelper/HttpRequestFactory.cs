using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Helpers.HttpRequestHelper
{
	/// <summary>
	/// Builds and executes an HTTPRequest.
	/// The first parameter is always an HttpRequestBuilder. If it is null, a new HtpClient will be created for the call.
	/// If it is not null, then the existing HttpClient will be used. 
	/// 
	/// Each method accepts the same 5 optional parameters. If nulled or left unspecified, default values will be used. For everything
	/// except AcceptHeader and Timeout, the defaults are nulls. 
	/// Acceptheader defaults to "application/json"
	/// Timeout defaults to new TimeSpan(0, 0, 15) - ie 15 seconds
	/// 
	/// In most cases these methods can be called with just the required parameters. 
	/// For instance: 
	///     Get(uri) or 
	///     Post(uri, dataobject)
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class HttpRequestFactory : IHttpRequestFactory
	{
		/// <summary>
		/// Provides access to the Builder
		/// </summary>
		public HttpRequestBuilder Builder { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="client"></param>
		public HttpRequestFactory(HttpClient client) => 
			this.Builder = new HttpRequestBuilder(client).Reset();

		/// <summary>
		/// Encapsulates and execute a Get call
		/// </summary>
		/// <param name="requestUri">The location of the endpoint.</param>
		/// <param name="completionOption">(Optional) An HtpCompletionOption - default: null</param>
		/// <param name="cancellationToken">(Optional) A CancellationToken -  default: null</param>
		/// <param name="bearerToken">(Optional) A string representing the BearerToken - default: null</param>
		/// <param name="acceptHeader">(Optional) A string representing the accepted data type - default: "application/json"</param>
		/// <param name="timeout">(Optional) A timeout value - default: 15 seconds</param>
		/// <param name="maxResponseSize">(Optional) A response content size  default: null</param>
		/// <returns>HttpResponseMessage</returns>
		public  async Task<HttpResponseMessage> GetAsync(
			Uri requestUri, 
			HttpCompletionOption? completionOption = null, 
			CancellationToken? cancellationToken = null,
			string bearerToken = null,
			string acceptHeader = null,
			TimeSpan? timeout = null,
			long? maxResponseSize = null)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException(nameof(requestUri));
			}

			this.Builder
				.SetCommonProperties(acceptHeader, bearerToken, cancellationToken, completionOption, timeout, maxResponseSize)
				.SetMethod(HttpMethod.Get)
				.SetRequestUri(requestUri)
				.SetContent(null);

			return await this.Builder.SendAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Encapsulates the Post call
		/// </summary>
		/// <param name="requestUri">The location of the endpoint.</param>
		/// <param name="value">The data object to Post</param>
		/// <param name="completionOption">(Optional) An HtpCompletionOption - default: null</param>
		/// <param name="cancellationToken">(Optional) A CancellationToken -  default: null</param>
		/// <param name="bearerToken">(Optional) A string representing the BearerToken - default: null</param>
		/// <param name="acceptHeader">(Optional) A string representing the accepted data type - default: "application/json"</param>
		/// <param name="timeout">(Optional) A timeout value - default: 15 seconds</param>
		/// <param name="maxResponseSize">(Optional) A response content size  default: null</param>
		/// <returns>HttpResponseMessage</returns>
		public  async Task<HttpResponseMessage> PostAsync(
			Uri requestUri,
			object value,
			HttpCompletionOption? completionOption = null,
			CancellationToken? cancellationToken = null,
			string bearerToken = null,
			string acceptHeader = null,
			TimeSpan? timeout = null,
			long? maxResponseSize = null)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException(nameof(requestUri));
			}

			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.Builder
				.SetCommonProperties(acceptHeader, bearerToken, cancellationToken, completionOption, timeout, maxResponseSize)
				.SetMethod(HttpMethod.Post)
				.SetRequestUri(requestUri)
				.SetContent(new JsonContent(value));

			return await this.Builder.SendAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Encapsulates a Put call 
		/// </summary>
		/// <param name="requestUri">The location of the endpoint.</param>
		/// <param name="value">The data object to Put</param>
		/// <param name="completionOption">(Optional) An HtpCompletionOption - default: null</param>
		/// <param name="cancellationToken">(Optional) A CancellationToken -  default: null</param>
		/// <param name="bearerToken">(Optional) A string representing the BearerToken - default: null</param>
		/// <param name="acceptHeader">(Optional) A string representing the accepted data type - default: "application/json"</param>
		/// <param name="timeout">(Optional) A timeout value - default: 15 seconds</param>
		/// <param name="maxResponseSize">(Optional) A response content size  default: null</param>
		/// <returns>HttpResponseMessage</returns>
		public  async Task<HttpResponseMessage> PutAsync(
			Uri requestUri, 
			object value,
			HttpCompletionOption? completionOption = null,
			CancellationToken? cancellationToken = null,
			string bearerToken = null,
			string acceptHeader = null,
			TimeSpan? timeout = null,
			long? maxResponseSize = null)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException(nameof(requestUri));
			}

			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.Builder
				.SetCommonProperties(acceptHeader, bearerToken, cancellationToken, completionOption, timeout, maxResponseSize)
				.SetMethod(HttpMethod.Put)
				.SetRequestUri(requestUri)
				.SetContent(new JsonContent(value));

			return await this.Builder.SendAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Encapsulates a Patch call
		/// </summary>
		/// <param name="requestUri">The location of the endpoint.</param>
		/// <param name="value">The data object to Patch</param>
		/// <param name="completionOption">(Optional) An HtpCompletionOption - default: null</param>
		/// <param name="cancellationToken">(Optional) A CancellationToken -  default: null</param>
		/// <param name="bearerToken">(Optional) A string representing the BearerToken - default: null</param>
		/// <param name="acceptHeader">(Optional) A string representing the accepted data type - default: "application/json"</param>
		/// <param name="timeout">(Optional) A timeout value - default: 15 seconds</param>
		/// <param name="maxResponseSize">(Optional) A response content size  default: null</param>
		/// <returns>HttpResponseMessage</returns>
		public  async Task<HttpResponseMessage> PatchAsync(
			Uri requestUri, 
			object value,
			HttpCompletionOption? completionOption = null,
			CancellationToken? cancellationToken = null,
			string bearerToken = null,
			string acceptHeader = null,
			TimeSpan? timeout = null,
			long? maxResponseSize = null)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException(nameof(requestUri));
			}

			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.Builder
				.SetCommonProperties(acceptHeader, bearerToken, cancellationToken, completionOption, timeout, maxResponseSize)
				.SetMethod(new HttpMethod("PATCH"))
				.SetRequestUri(requestUri)
				.SetContent(new PatchContent(value));

			return await this.Builder.SendAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Encapsulates a Delete call
		/// </summary>
		/// <param name="requestUri">The location of the endpoint.</param>
		/// <param name="completionOption">(Optional) An HtpCompletionOption - default: null</param>
		/// <param name="cancellationToken">(Optional) A CancellationToken -  default: null</param>
		/// <param name="bearerToken">(Optional) A string representing the BearerToken - default: null</param>
		/// <param name="acceptHeader">(Optional) A string representing the accepted data type - default: "application/json"</param>
		/// <param name="timeout">(Optional) A timeout value - default: 15 seconds</param>
		/// <param name="maxResponseSize">(Optional) A response content size  default: null</param>
		/// <returns>HttpResponseMessage</returns>
		public  async Task<HttpResponseMessage> DeleteAsync(
			Uri requestUri,
			HttpCompletionOption? completionOption = null,
			CancellationToken? cancellationToken = null,
			string bearerToken = null,
			string acceptHeader = null,
			TimeSpan? timeout = null,
			long? maxResponseSize = null)
		{
			if (requestUri == null)
			{
				throw new ArgumentNullException(nameof(requestUri));
			}

			this.Builder
				.SetCommonProperties(acceptHeader, bearerToken, cancellationToken, completionOption, timeout, maxResponseSize)
				.SetMethod(HttpMethod.Delete)
				.SetRequestUri(requestUri)
				.SetContent(null);

			return await this.Builder.SendAsync().ConfigureAwait(false);
		}		
	}	
}
