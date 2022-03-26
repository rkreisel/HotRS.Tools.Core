using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using RK.HotRS.ToolsCore.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RK.HotRS.ToolsCore.Middleware.SwaggerTools
{
    /// <summary>    
    /// Enable handling file uploads for IFormFile types via swagger.    
    /// </summary>    
    public class FormFileParameter : IOperationFilter
    {
        /// <summary>
        /// Apply the file upload in Swagger        
        /// </summary>        
        /// <param name="operation">The operation<see cref="OpenApiOperation"/></param>        
        /// <param name="context">The context<see cref="OperationFilterContext"/></param>     
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.CheckForNull(nameof(operation));
            if (operation.OperationId == "MyOperation")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "formFile",
                    In = ParameterLocation.Header,
                    Description = "Upload File",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "file",
                        Format = "binary"
                    }
                });
                var uploadFileMediaType = new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "object",
                        Properties =
                        {
                            ["uploadedFile"] = new OpenApiSchema()
                            {
                                Description = "Upload File",
                                Type = "file",
                                Format = "binary"

                            }
                        },
                        Required = new HashSet<string>()
                        {
                            "uploadedFile"
                        }
                    }
                };
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                {
                    ["multipart/form-data"] = uploadFileMediaType
                }
                };
            }
        }
        //public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //{
        //    operation.CheckForNull(nameof(operation));
        //    context.CheckForNull(nameof(context));
        //    var parameters = operation.Parameters;
        //    if (parameters == null || parameters.Count == 0) return;
        //    var formFileParameterNames = new List<string>();
        //    var formFileSubParameterNames = new List<string>();
        //    foreach (var actionParameter in context.ApiDescription.ActionDescriptor.Parameters)
        //    {
        //        var properties = actionParameter.ParameterType.GetProperties().Where(p => p.PropertyType == typeof(IFormFile))
        //            .Select(p => p.Name).ToArray();
        //        if (properties.Length != 0)
        //        {
        //            formFileParameterNames.AddRange(properties);
        //            formFileSubParameterNames.AddRange(properties);
        //            continue;
        //        }
        //        if (actionParameter.ParameterType != typeof(IFormFile))
        //            continue;
        //        formFileParameterNames.Add(actionParameter.Name);
        //    }
        //    if (!formFileParameterNames.Any())
        //        return;

        //    var consumes = operation.Consumes;
        //    consumes.Clear();
        //    consumes.Add(formDataMimeType);

        //    foreach (var parameter in parameters.ToArray())
        //    {
        //        if (!(parameter is NonBodyParameter) || parameter.In != "formData") continue;
        //        if (formFileSubParameterNames.Any(p => parameter.Name.StartsWith(p + ".", StringComparison.InvariantCultureIgnoreCase)) || formFilePropertyNames.Contains(parameter.Name))
        //            parameters.Remove(parameter);
        //    }
        //    foreach (var formFileParameter in formFileParameterNames)
        //    {
        //        parameters.Add(new NonBodyParameter()
        //        {
        //            Name = formFileParameter,
        //            Type = "file",
        //            In = "formData"
        //        });
        //    }
        //}
    }
}


