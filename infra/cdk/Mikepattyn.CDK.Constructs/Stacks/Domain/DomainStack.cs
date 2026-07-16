namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Hosts one <see cref="IPlatformDomain"/> import construct for CloudFront custom domains.
/// </summary>
public class DomainStack : Stack
{
    public IPlatformDomain PlatformDomain { get; }

    public string DomainName => PlatformDomain.DomainName;
    public IHostedZone HostedZone => PlatformDomain.HostedZone;
    public ICertificate Certificate => PlatformDomain.Certificate;

    public DomainStack(Construct scope, DomainStackProps props)
        : base(scope, props.StackId, new StackProps { Env = props.StackEnvironment })
    {
        PlatformDomain = props.CreatePlatformDomain(
            this,
            props.GetUniqueResourceId(nameof(PlatformDomain)),
            new PlatformDomainConstructProps
            {
                HostedZoneId = props.HostedZoneId,
                CertificateArn = props.CertificateArn,
            }
        );
    }
}
