namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Shared import-only wiring for a platform apex domain (hosted zone + ACM certificate).
/// </summary>
public abstract class PlatformDomainConstruct : Construct, IPlatformDomain
{
    public string DomainName { get; }
    public IHostedZone HostedZone { get; }
    public ICertificate Certificate { get; }

    protected PlatformDomainConstruct(
        Construct scope,
        string id,
        string domainName,
        PlatformDomainConstructProps props
    )
        : base(scope, id)
    {
        DomainName = domainName;

        HostedZone = Amazon.CDK.AWS.Route53.HostedZone.FromHostedZoneAttributes(
            this,
            nameof(HostedZone),
            new HostedZoneAttributes
            {
                HostedZoneId = props.HostedZoneId,
                ZoneName = domainName,
            }
        );

        Certificate = Amazon.CDK.AWS.CertificateManager.Certificate.FromCertificateArn(
            this,
            nameof(Certificate),
            props.CertificateArn
        );

        AmazonAspect.Of(this).Add(new ImportedOnlyConstructAspect(id));
    }
}
