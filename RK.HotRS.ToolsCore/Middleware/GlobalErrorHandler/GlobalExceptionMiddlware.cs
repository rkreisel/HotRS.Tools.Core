namespace HotRS.Tools.Core.Middleware.GlobalErrorHandler;

/// <summary>
/// Provides a Global handler for uncaught exceptions.
/// </summary>
[ExcludeFromCodeCoverage]
public class GlobalExceptionMiddleware : IGlobalExceptionExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<GlobalExceptionMiddleware> logger;
    private readonly GlobalExceptionHandlerOptions options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="options"></param>
    public GlobalExceptionMiddleware(RequestDelegate next, GlobalExceptionHandlerOptions options, ILoggerFactory loggerFactory)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        logger = loggerFactory?.CreateLogger<GlobalExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        if (options == null)
        {
            this.options = new GlobalExceptionHandlerOptions();
        }
        else
        {
            this.options = options;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                logger.LogWarning(Resources.RESPONSEALREADYSTARTED);
                throw;
            }
            context.Response.Clear();
            context.Response.StatusCode = (int)options.StatusCode;
            context.Response.ContentType = options.ContentType;

            if (context.Response.ContentType.Contains("json", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.ContentType = options.ContentType;
                if (options.FullDetail)
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(ex)).ConfigureAwait(false);
                else
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(BuildCustomMessage(ex))).ConfigureAwait(false);
                }
            }
            else if (context.Response.ContentType.Contains("XML", StringComparison.InvariantCultureIgnoreCase))
            {
                if (options.FullDetail)
                    await context.Response.WriteAsync(XmlConverter.SerializeObject(ConvertCustomMessageToXML(BuildCustomMessage(ex, true)))).ConfigureAwait(false);
                else
                    await context.Response.WriteAsync(XmlConverter.SerializeObject(ConvertCustomMessageToXML(BuildCustomMessage(ex)))).ConfigureAwait(false);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                if (options.FullDetail)
                    await context.Response.WriteAsync(ex.ToString()).ConfigureAwait(false);
                else

                    await context.Response.WriteAsync(string.Join(Environment.NewLine, BuildCustomMessage(ex).ToArray())).ConfigureAwait(false);
            }

            logger.LogError(ex.ToString());

            return;
        }
    }

    private IDictionary<string, string> BuildCustomMessage(Exception ex, bool forceAll = false)
    {
        try
        {
            var result = new Dictionary<string, string> { { "Message", ex.Message } };
            if (options.IncludeData || forceAll)
                result.Add("Data", JsonConvert.SerializeObject(ex.Data));
            if (options.IncludeHelpLink || forceAll)
                result.Add("HelpLink", ex.HelpLink);
            if (options.IncludeHResult || forceAll)
                result.Add("HResult", ex.HResult.ToString(CultureInfo.InvariantCulture));
            if (options.IncludeInnerException || forceAll)
            {
                if (ex.InnerException != null)
                    result.Add("InnerException", JsonConvert.SerializeObject(BuildCustomMessage(ex.InnerException)));
            }
            if (options.IncludeSource || forceAll)
                result.Add("Source", ex.Source);
            if (options.IncludeStackTrace || forceAll)
                result.Add("StackTrace", ex.StackTrace);
            return result;
        }
        catch
        {
            return null;
        }
    }

    private static XmlDocument ConvertCustomMessageToXML(IDictionary<string, string> source)
    {
        var doc = new XmlDocument();
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlElement root = doc.DocumentElement;
        doc.InsertBefore(xmlDeclaration, root);


        var errorDetail = doc.CreateElement(string.Empty, "ExceptionDetail", string.Empty);
        doc.AppendChild(errorDetail);

        foreach (var item in source)
        {
            var element = doc.CreateElement(string.Empty, item.Key.Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase), string.Empty);
            var value = doc.CreateTextNode(item.Value);
            element.AppendChild(value);
            errorDetail.AppendChild(element);
        }
        return doc;
    }
}


/// <summary>
/// Extension method used to add the middleware to the HTTP request pipeline. 
/// </summary>
public static class GlobalExceptionMiddlewareExtensions
{
    /// <summary>
    /// Implementation:
    /// app.UseGlobalExceptionMiddleware(options => 
    ///		{
    ///			options.ContentType = "text.json";
    ///         options.StatusCode = HttpStatusCode.InternalServerError;
    ///         options.FullDetail = false;
    ///         options.IncludeData = false;
    ///         options.IncludeHelpLink = false;
    ///         options.IncludeHResult = false;
    ///         options.IncludeInnerException = false;
    ///         options.IncludeSource = false;
    ///         options.IncludeStackTrace = false;
    ///      });
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    public static void UseGlobalExceptionMiddleware(
        this IApplicationBuilder builder,
        Action<GlobalExceptionHandlerOptions> configureOptions = null)
    {
        var options = new GlobalExceptionHandlerOptions();
        configureOptions?.Invoke(options);
        builder.UseMiddleware<GlobalExceptionMiddleware>(options);
    }
}

/// <summary>
/// A class to hold parameters to be passed to the GlobalExceptionHandler constuctor
/// </summary>
public class GlobalExceptionHandlerOptions
{
    /// <summary>
    /// One of the standard Http Content Type strings
    /// (e.g. 'text/json')
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// One of the standard Http Status Codes
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// If true then entire exception is displayed, otherwise only the Selected properties are displayed
    /// </summary>
    public bool FullDetail { get; set; }

    /// <summary>
    /// If true  then Data property of the exception is displayed.
    /// Default = false
    /// Note: Only valid if FullDetail is false.
    /// </summary>
    public bool IncludeData { get; set; }

    /// <summary>
    /// If true  then HelpLink property of the exception is displayed.
    /// Default = false
    /// Note: Only valid if FullDetail is false.
    /// </summary>
    public bool IncludeHelpLink { get; set; }

    /// <summary>
    /// If true  then HResult property of the exception is displayed.
    /// Default = false
    /// Note: Only valid if FullDetail is false.
    /// </summary>
    public bool IncludeHResult { get; set; }

    /// <summary>
    /// If true  then InnerException property of the exception is displayed.
    /// Default = false
    /// Note: Only valid if FullDetail is false.
    /// </summary>
    public bool IncludeInnerException { get; set; }

    /// <summary>
    /// If true  then Source property of the exception is displayed.
    /// Default = false
    /// Note: Only valid if FullDetail is false.
    /// </summary>
    public bool IncludeSource { get; set; }

    /// <summary>
    /// If true  then StackTrace property of the exception is displayed.
    /// Default = false
    /// Note: Only valid if FullDetail is false.
    /// </summary>
    public bool IncludeStackTrace { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public GlobalExceptionHandlerOptions()
    {
        ContentType = "text/json";
        FullDetail = false;
        StatusCode = HttpStatusCode.BadRequest;
        IncludeData = false;
        IncludeHelpLink = false;
        IncludeHResult = false;
        IncludeInnerException = false;
        IncludeSource = false;
        IncludeStackTrace = false;
    }
}
