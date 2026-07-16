namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Hostnames for Production apex brand sites (portfolio and separate brand domains).
/// </summary>
public static class BrandHostnames
{
    public static string Primary(string platformDomain) => platformDomain;

    public static string Www(string platformDomain) => $"www.{platformDomain}";

    public static string[] GetDomainNames(string platformDomain) =>
        [Primary(platformDomain), Www(platformDomain)];
}
