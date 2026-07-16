using Amazon.CDK.AWS.Route53.Targets;

namespace Mikepattyn.CDK.Constructs;

public class FishEdgeStackProps : BaseStackProps
{
    public required string AppSlug { get; init; }
    public required string PlatformDomainName { get; init; }
    public required IHostedZone HostedZone { get; init; }
    public required ICertificate Certificate { get; init; }
    public required string ApiGatewayDomainName { get; init; }
    protected override string StackName => Constants.Stacks.FishFrontend;
}
