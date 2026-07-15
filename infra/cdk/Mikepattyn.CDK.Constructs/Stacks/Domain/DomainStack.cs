namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Imports existing Route53 hosted zone and ACM certificate for CloudFront custom domains.
/// </summary>
public class DomainStack : Stack
{
    public string DomainName { get; }
    public IHostedZone HostedZone { get; }
    public ICertificate Certificate { get; }

    public DomainStack(Construct scope, DomainStackProps props)
        : base(scope, props.StackId, new StackProps { Env = props.StackEnvironment })
    {
        DomainName = props.DomainName;
        HostedZone = Amazon.CDK.AWS.Route53.HostedZone.FromHostedZoneAttributes(
            this,
            props.GetUniqueResourceId(nameof(HostedZone)),
            new HostedZoneAttributes
            {
                HostedZoneId = props.HostedZoneId,
                ZoneName = props.DomainName,
            }
        );

        Certificate = Amazon.CDK.AWS.CertificateManager.Certificate.FromCertificateArn(
            this,
            props.GetUniqueResourceId(nameof(Certificate)),
            props.CertificateArn
        );

        AmazonAspect.Of(this).Add(new ImportedOnlyConstructAspect(props.StackId));
    }
}
