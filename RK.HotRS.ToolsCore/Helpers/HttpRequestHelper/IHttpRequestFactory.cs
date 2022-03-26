namespace HotRS.Tools.Core.Helpers.HttpRequestHelper;

/// <summary>
/// Interface for the HttpRequestFactory
/// </summary>
public interface IHttpRequestFactory
{
    /// <summary>
    /// A Builder object
    /// </summary>
    HttpRequestBuilder Builder { get; }

    /// <summary>
    /// Implements the Delete call
    /// </summary>
    /// <param name="requestUri">The Uri to call</param>
    /// <param name="completionOption">The (optional) CompletionOption</param>
    /// <param name="cancellationToken">The (optional) CancellationToken</param>
    /// <param name="bearerToken">The (optional) BearerToken</param>
    /// <param name="acceptHeader">The (optional) AcceptHeader</param>
    /// <param name="timeout">The (optional) Timeou</param>
    /// <param name="maxResponseSize">The (optional) MaxResponseSize </param>
    /// <returns></returns>
    Task<HttpResponseMessage> DeleteAsync(Uri requestUri, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null, string bearerToken = null, string acceptHeader = null, TimeSpan? timeout = null, long? maxResponseSize = null);

    /// <summary>
    /// Implements the Get call
    /// </summary>
    /// <param name="requestUri">The Uri to call</param>
    /// <param name="completionOption">The (optional) CompletionOption</param>
    /// <param name="cancellationToken">The (optional) CancellationToken</param>
    /// <param name="bearerToken">The (optional) BearerToken</param>
    /// <param name="acceptHeader">The (optional) AcceptHeader</param>
    /// <param name="timeout">The (optional) Timeou</param>
    /// <param name="maxResponseSize">The (optional) MaxResponseSize </param>
    /// <returns></returns>
    Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null, string bearerToken = null, string acceptHeader = null, TimeSpan? timeout = null, long? maxResponseSize = null);

    /// <summary>
    /// Implements the Patch call
    /// </summary>
    /// <param name="requestUri">The Uri to call</param>
    /// <param name="value">The data to pass in</param>
    /// <param name="completionOption">The (optional) CompletionOption</param>
    /// <param name="cancellationToken">The (optional) CancellationToken</param>
    /// <param name="bearerToken">The (optional) BearerToken</param>
    /// <param name="acceptHeader">The (optional) AcceptHeader</param>
    /// <param name="timeout">The (optional) Timeou</param>
    /// <param name="maxResponseSize">The (optional) MaxResponseSize </param>
    /// <returns></returns>
    Task<HttpResponseMessage> PatchAsync(Uri requestUri, object value, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null, string bearerToken = null, string acceptHeader = null, TimeSpan? timeout = null, long? maxResponseSize = null);

    /// <summary>
    /// Implements the Post call
    /// </summary>
    /// <param name="requestUri">The Uri to call</param>
    /// <param name="value">The data to pass in</param>
    /// <param name="completionOption">The (optional) CompletionOption</param>
    /// <param name="cancellationToken">The (optional) CancellationToken</param>
    /// <param name="bearerToken">The (optional) BearerToken</param>
    /// <param name="acceptHeader">The (optional) AcceptHeader</param>
    /// <param name="timeout">The (optional) Timeou</param>
    /// <param name="maxResponseSize">The (optional) MaxResponseSize </param>
    /// <returns></returns>
    Task<HttpResponseMessage> PostAsync(Uri requestUri, object value, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null, string bearerToken = null, string acceptHeader = null, TimeSpan? timeout = null, long? maxResponseSize = null);

    /// <summary>
    /// Implements the Put call
    /// </summary>
    /// <param name="requestUri">The Uri to call</param>
    /// <param name="value">The data to pass in</param>
    /// <param name="completionOption">The (optional) CompletionOption</param>
    /// <param name="cancellationToken">The (optional) CancellationToken</param>
    /// <param name="bearerToken">The (optional) BearerToken</param>
    /// <param name="acceptHeader">The (optional) AcceptHeader</param>
    /// <param name="timeout">The (optional) Timeou</param>
    /// <param name="maxResponseSize">The (optional) MaxResponseSize </param>
    /// <returns></returns>
    Task<HttpResponseMessage> PutAsync(Uri requestUri, object value, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null, string bearerToken = null, string acceptHeader = null, TimeSpan? timeout = null, long? maxResponseSize = null);
}
