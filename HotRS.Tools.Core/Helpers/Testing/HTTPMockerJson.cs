using Moq;
using Moq.Protected;

namespace SmartScanUITests.Helpers;

public static class HTTPMockerJson
{
    /// <summary>
    /// Helper method to create an HTTPClient with a predefined JSON result
    /// </summary>
    /// <param name="jsonContentToReturn">The HTTPStatusCode to return. Defaults to Ok</param>
    /// <param name="returnStatus">The desired data to return.</param>
    /// <param name="reasonMessage">The failure reason if mocking a failed request.</param>
    /// <param name="baseAddress"></param>
    /// Usage:
    /// First, create an object containing the data you wish to return.
    /// Note that the object must be JSON serializable.
    /// var data = new Whatever {Propert1 = "Hello", Property3 = "World"};
    /// Second, create an instance of the HTTPMockerJson class that will return the desire string of JSON data.
    /// var mockedResult = new HTTPMickerJson(data) - optionally provide nonsuccess status codes and/or a different url)
    /// Note that providing a different url would only be useful if you were going to make an Assertion about the returned url.
    /// Third, use the mocked instance when creating the instance of the class under test.
    /// <returns>HTTPClient which will, when executed, returns the string content of the json object.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public static async Task<HttpClient> HTTPMockedClient(
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        object jsonContentToReturn,
        HttpStatusCode returnStatus = HttpStatusCode.OK,
        string reasonMessage = "",
        string baseAddress = "http://test.com/")
    {
        StringContent contentStr;
        if (jsonContentToReturn != null)
        {
            contentStr = new StringContent(System.Text.Json.JsonSerializer.Serialize(jsonContentToReturn));
        }
        else
        {
            contentStr = new StringContent(string.Empty);
        }

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
           .Protected()
           // Setup the PROTECTED method to mock
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           // prepare the expected response of the mocked http call
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = returnStatus,
               Content = contentStr,
               ReasonPhrase = reasonMessage
           })
           .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(baseAddress),
        };

        return httpClient;
    }
}
