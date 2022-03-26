using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using RK.HotRS.ToolsCore.Extensions;

namespace RK.HotRS.ToolsCore.Helpers.HttpRequestHelper
{
    /// <summary>
    /// A builder for HTTP requests
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class HttpRequestBuilder
    {
        /// <summary>
        /// The Http method (Put. Post, etc)
        /// </summary>
        public HttpMethod Method { get; private set; }
        /// <summary>
        /// A URI pointing to the method to call
        /// </summary>
        public Uri RequestUri { get; private set; }
        /// <summary>
        /// The content to send
        /// </summary>
        public HttpContent Content { get; private set; }
        /// <summary>
        /// A Bearer Token
        /// </summary>
        public string BearerToken { get; private set; }
        /// <summary>
        /// The AcceptHeader
        /// </summary>
        public string AcceptHeader { get; private set; }
        /// <summary>
        /// The timeout to use. 
        ///Note: Set this ONLY once
        /// </summary>
        public TimeSpan Timeout { get; private set; }
        /// <summary>
        /// A  Cancellation token (optional)
        /// </summary>
        public CancellationToken? CancellationToken { get; private set; }
        /// <summary>
        /// A completion optipon
        /// </summary>
        public HttpCompletionOption? HttpCompletionOption { get; private set; }
        /// <summary>
        /// Maximum sice of the response object
        /// </summary>
        public long? MaxResponseSize { get; private set; }
        /// <summary>
        /// The HttpClient
        /// </summary>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public HttpRequestBuilder(HttpClient client)
        {
            this.Client = client;
            Reset();
        }

        /// <summary>
        /// A method to reset the object
        /// </summary>
        /// <returns></returns>
        public HttpRequestBuilder Reset()
        {
            Method = null;
            RequestUri = null;
            Content = null;
            BearerToken = string.Empty;
            AcceptHeader = "application/json";
            Timeout = new TimeSpan(0, 0, 15);
            CancellationToken = null;
            HttpCompletionOption = null;
            MaxResponseSize = null;
            return this;
        }

        /// <summary>
        /// Sets the Method parameter
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetMethod(HttpMethod method)
        {
            Method = method;
            return this;
        }

        /// <summary>
        /// Sets the RequestUri
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetRequestUri(Uri requestUri)
        {
            RequestUri = requestUri;
            return this;
        }

        /// <summary>
        /// Set the Content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetContent(HttpContent content)
        {
            Content = content;
            return this;
        }

        /// <summary>
        /// Set the BearerToken
        /// </summary>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetBearerToken(string bearerToken)
        {
            BearerToken = bearerToken;
            return this;
        }

        /// <summary>
        /// Set the AcceptHeader
        /// </summary>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetAcceptHeader(string acceptHeader)
        {
            AcceptHeader = string.IsNullOrWhiteSpace(acceptHeader) ? "application/json" : acceptHeader;
            return this;
        }

        /// <summary>
        /// Set the Timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetTimeout(TimeSpan? timeout)
        {
            Timeout = timeout == null ? new TimeSpan(0, 0, 15) : timeout.Value;
            return this;
        }

        /// <summary>
        /// Set the CancellationToken
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetCancellationToken(CancellationToken? cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }

        /// <summary>
        /// Set the HttpCompletion
        /// </summary>
        /// <param name="httpCompletionToken"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetHttpCompletionOption(HttpCompletionOption? httpCompletionToken)
        {
            HttpCompletionOption = httpCompletionToken;
            return this;
        }

        /// <summary>
        /// Set the max response size
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpRequestBuilder SetMaxResponseSize(long? value)
        {
            MaxResponseSize = value;
            return this;
        }

        /// <summary>
        /// The primary method that executes the call.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            Method.CheckForNull(nameof(Method));
            RequestUri.CheckForNull(nameof(RequestUri));
            
            // Setup request
            using (var request = new HttpRequestMessage())
            {
                //Method = this.Method,
				//RequestUri = this.RequestUri
            //};

                if (Content != null)
                {
                    request.Content = Content;
                }

                if (!string.IsNullOrWhiteSpace(BearerToken))
                {
                    request.Headers.Authorization =
                      new AuthenticationHeaderValue("Bearer", BearerToken);
                }

                request.Headers.Accept.Clear();
                if (!string.IsNullOrWhiteSpace(AcceptHeader))
                {
                    request.Headers.Accept.Add(
                       new MediaTypeWithQualityHeaderValue(AcceptHeader));
                }

                // Setup client			
                if (Client.Timeout != Timeout)
                {
                    Client.Timeout = Timeout;
                }

                if (MaxResponseSize != null)
                {
                    Client.MaxResponseContentBufferSize = MaxResponseSize.Value;
                }

                //use available (non-null) call parameters
                if (HttpCompletionOption == null && CancellationToken == null) //have neither parameter
                {
                    return await Client.SendAsync(request).ConfigureAwait(false);
                }
                else if (HttpCompletionOption == null && CancellationToken != null) //have only CancellationToken
                {
                    return await Client.SendAsync(request, CancellationToken.Value).ConfigureAwait(false);
                }
                else if (CancellationToken == null && HttpCompletionOption != null) //have only HttpCompletionOption
                {
                    return await Client.SendAsync(request, HttpCompletionOption.Value).ConfigureAwait(false);
                }
                else  //have both parameters
                {
                    return await Client.SendAsync(request, HttpCompletionOption.Value, CancellationToken.Value).ConfigureAwait(false);
                }
            };
        }

    }
}
