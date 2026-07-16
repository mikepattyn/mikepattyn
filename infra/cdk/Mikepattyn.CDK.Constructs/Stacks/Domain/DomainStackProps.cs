namespace Mikepattyn.CDK.Constructs;

public class DomainStackProps : BaseStackProps
{
    public required string HostedZoneId { get; init; }
    public required string CertificateArn { get; init; }
    public required string PlatformStackName { get; init; }

    public required Func<
        Construct,
        string,
        PlatformDomainConstructProps,
        IPlatformDomain
    > CreatePlatformDomain { get; init; }

    protected override string StackName => PlatformStackName;
}
