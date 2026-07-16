namespace Mikepattyn.CDK.Constructs;

public sealed class MikepattynPlatformDomainConstruct : PlatformDomainConstruct
{
    public MikepattynPlatformDomainConstruct(
        Construct scope,
        string id,
        PlatformDomainConstructProps props
    )
        : base(scope, id, Constants.Domains.Mikepattyn, props) { }
}
