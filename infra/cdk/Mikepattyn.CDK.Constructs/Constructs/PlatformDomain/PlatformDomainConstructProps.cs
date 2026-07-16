namespace Mikepattyn.CDK.Constructs;

public class PlatformDomainConstructProps
{
    public required string HostedZoneId { get; init; }
    public required string CertificateArn { get; init; }
}
