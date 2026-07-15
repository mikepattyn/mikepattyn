namespace Mikepattyn.CDK.Constructs;

public class ApiGatewayConstructProps : BaseConstructProps
{
    public required LambdaConstruct LambdaConstruct { get; init; }
    public required AuthorizersConstruct AuthorizersConstruct { get; init; }
    public required ApiRateLimitOptions RateLimitOptions { get; init; }
    protected override string ConstructName => "ApiGateway";
}
