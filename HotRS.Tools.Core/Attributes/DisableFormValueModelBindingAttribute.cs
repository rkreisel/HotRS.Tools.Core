namespace HotRS.Tools.Core.Attributes;

/// <summary>
/// Provides an atribute to disable form value model binding when using the file upload method
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
[ExcludeFromCodeCoverage]
public sealed class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
{
    /// <summary>
    /// Intercepts the OnResourceExecuting method 
    /// </summary>
    /// <param name="context"></param>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        var factories = context.ValueProviderFactories;
        factories.RemoveType<FormValueProviderFactory>();
        factories.RemoveType<FormFileValueProviderFactory>();
        factories.RemoveType<JQueryFormValueProviderFactory>();
    }

    /// <summary>
    /// Intercepts the OnResourceExecuted method
    /// </summary>
    /// <param name="context"></param>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}
