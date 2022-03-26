namespace HotRS.Tools.Core.Middleware.SwaggerTools.Tests;

[TestFixture()]
[ExcludeFromCodeCoverage]
public class FormFileParameterTests
{
    [Test()]
    public void ApplyTestNullOperationFails()
    {
        var openApiOperation = new OpenApiOperation();
        var operationFilterContext = new OperationFilterContext(null, null, null, null);
        var obj2Test = new FormFileParameter();
        Assert.That(() => obj2Test.Apply(null, operationFilterContext),
            Throws.Exception.TypeOf<ArgumentNullException>()
            .With.Message.EqualTo("Value cannot be null. (Parameter 'operation')"));
    }

    [Test()]
    public void ApplyTestNullOperationNotMyOperation()
    {
        var openApiOperation = new OpenApiOperation { OperationId = "YourOperation" };
        var operationFilterContext = new OperationFilterContext(null, null, null, null);
        var obj2Test = new FormFileParameter();
        Assert.DoesNotThrow(() => obj2Test.Apply(openApiOperation, operationFilterContext));
    }

    [Test()]
    public void ApplyTestNullOperationMyOperation()
    {
        var openApiOperation = new OpenApiOperation { OperationId = "MyOperation" };
        var operationFilterContext = new OperationFilterContext(null, null, null, null);
        var obj2Test = new FormFileParameter();
        Assert.DoesNotThrow(() => obj2Test.Apply(openApiOperation, operationFilterContext));
        Assert.That(openApiOperation.Parameters.Count > 0);
        var firstParam = openApiOperation.Parameters.First();
        Assert.That(firstParam.Name.Equals("formfile", StringComparison.OrdinalIgnoreCase));
        Assert.That(firstParam.Schema.Type.Equals("file", StringComparison.OrdinalIgnoreCase));
    }
}
