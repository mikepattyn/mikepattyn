namespace Mikepattyn.CDK.Constructs;

public class LambdaConstructProps : BaseConstructProps
{
    protected override string ConstructName => "Lambda";
    public required DynamoDbConstruct DynamoDbConstruct { get; init; }
    public required string AuthressApiBasePath { get; init; }
    public required string AuthressResourceGroupId { get; init; }
}
