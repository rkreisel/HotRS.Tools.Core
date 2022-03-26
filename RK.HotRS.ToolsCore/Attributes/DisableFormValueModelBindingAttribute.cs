using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RK.HotRS.ToolsCore.Extensions;

namespace RK.HotRS.ToolsCore.Attributes
{
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
			context.CheckForNull(nameof(context));
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
}
