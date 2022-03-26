namespace HotRS.Tools.Core.Middleware.SwaggerTools;

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
        ArgumentNullException.ThrowIfNull(operation,nameof(operation));
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
}


