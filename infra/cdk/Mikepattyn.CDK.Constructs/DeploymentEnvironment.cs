namespace Mikepattyn.CDK.Constructs;

public readonly struct DeploymentEnvironment
{
    public string Name { get; }
    public string? Subdomain { get; }

    public DeploymentEnvironment(string name, string? subdomain)
    {
        Name = name;
        Subdomain = subdomain;
    }

    public static readonly DeploymentEnvironment Development = new("Development", "dev");
    public static readonly DeploymentEnvironment Staging = new("Staging", "acc");
    public static readonly DeploymentEnvironment Production = new("Production", null);
    public static readonly DeploymentEnvironment None = new("None", null);

    public string GetFullyQualifiedDomainName(string domainName) =>
        Subdomain is null ? domainName : $"{Subdomain}.{domainName}";

    public string[] GetDomainNames(string domainName) =>
        [GetFullyQualifiedDomainName(domainName)];

    public string GetCnameRecordName(string domainName)
    {
        if (!IsValidDeploymentEnvironment)
        {
            throw new InvalidOperationException(
                "CnameRecordName is only valid for valid deployment environments"
            );
        }

        if (Subdomain is null)
        {
            throw new InvalidOperationException(
                "CnameRecordName is only valid for subdomain environments"
            );
        }

        return $"{Subdomain}.{domainName}";
    }

    public bool UsesApexDomain => Subdomain is null;

    public bool IsProduction => this == Production;
    public bool IsStaging => this == Staging;
    public bool IsDevelopment => this == Development;
    public bool IsValidDeploymentEnvironment => this != None;

    public static bool operator ==(DeploymentEnvironment left, DeploymentEnvironment right) =>
        left.Equals(right);

    public static bool operator !=(DeploymentEnvironment left, DeploymentEnvironment right) =>
        !left.Equals(right);

    public override bool Equals(object? obj) =>
        obj is DeploymentEnvironment other && Equals(other);

    public bool Equals(DeploymentEnvironment other) =>
        Name == other.Name && Subdomain == other.Subdomain;

    public override int GetHashCode() => HashCode.Combine(Name, Subdomain);
}
