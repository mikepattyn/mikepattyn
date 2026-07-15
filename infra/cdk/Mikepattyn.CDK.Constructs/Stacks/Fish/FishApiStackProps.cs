namespace Mikepattyn.CDK.Constructs;

public class FishApiStackProps : BaseStackProps
{
    public required IVpc Vpc { get; init; }
    protected override string StackName => Constants.Stacks.FishApi;
}
