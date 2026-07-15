namespace Mikepattyn.CDK.Constructs;

public class DomainStackProps : BaseStackProps
{
    public required string DomainName { get; init; }
    public required string HostedZoneId { get; init; }
    public required string CertificateArn { get; init; }
    protected override string StackName => Constants.Stacks.Domain;
}
