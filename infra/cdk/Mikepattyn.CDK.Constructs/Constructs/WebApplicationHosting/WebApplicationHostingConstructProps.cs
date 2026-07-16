namespace Mikepattyn.CDK.Constructs;

public class WebApplicationHostingConstructProps : BaseConstructProps
{
    public required string[] DomainNames { get; init; }
    public required ICertificate Certificate { get; init; }
    protected override string ConstructName => $"WebApp-{AppName}";
}
