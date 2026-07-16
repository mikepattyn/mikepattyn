namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Imported Route53 hosted zone and ACM certificate for one platform apex domain.
/// </summary>
public interface IPlatformDomain
{
    string DomainName { get; }
    IHostedZone HostedZone { get; }
    ICertificate Certificate { get; }
}
