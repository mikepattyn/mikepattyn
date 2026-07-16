namespace Mikepattyn.CDK.Constructs;

public sealed class AlienButNicePlatformDomainConstruct : PlatformDomainConstruct
{
    public AlienButNicePlatformDomainConstruct(
        Construct scope,
        string id,
        PlatformDomainConstructProps props
    )
        : base(scope, id, Constants.Domains.AlienButNice, props) { }
}
