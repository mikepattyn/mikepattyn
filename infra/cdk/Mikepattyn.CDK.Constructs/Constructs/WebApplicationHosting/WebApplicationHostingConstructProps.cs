namespace Mikepattyn.CDK.Constructs;

public class WebApplicationHostingConstructProps : BaseConstructProps
{
    public required string AppSlug { get; init; }
    public required string PlatformDomainName { get; init; }
    public required ICertificate Certificate { get; init; }
    protected override string ConstructName => $"WebApp-{AppName}";
}
