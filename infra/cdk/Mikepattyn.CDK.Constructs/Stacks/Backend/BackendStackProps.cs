namespace Mikepattyn.CDK.Constructs;

public class BackendStackProps : BaseStackProps
{
    public required string AuthressApiBasePath { get; init; }
    public required string AuthressResourceGroupId { get; init; }
    protected override string StackName => Constants.Stacks.KapsalonBackend;
}
