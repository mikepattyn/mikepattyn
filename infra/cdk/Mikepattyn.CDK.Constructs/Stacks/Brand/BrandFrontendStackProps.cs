namespace Mikepattyn.CDK.Constructs;

public class BrandFrontendStackProps : BaseStackProps
{
    public required string AppName { get; init; }
    public required string PlatformDomainName { get; init; }
    public required IHostedZone HostedZone { get; init; }
    public required ICertificate Certificate { get; init; }

    protected override string StackName =>
        Constants.Stacks.GetAppStack(AppName, "BrandFrontend");
}
