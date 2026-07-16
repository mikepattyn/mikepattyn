namespace Mikepattyn.CDK.E2E.Tests;

/// <summary>
/// Product contract for platform apex domains and application hostnames under mikepattyn.nl.
/// </summary>
public static class ExpectedDomains
{
    public sealed record PlatformDomainExpectation(string DomainName, string StackId);

    public sealed record AppHostnameExpectation(string Fqdn, string AppSlug);

    internal static readonly PlatformDomainExpectation[] PlatformDomains =
    [
        new("mikepattyn.nl", "Mikepattyn-Domain-Stack"),
        new("alienbutnice.nl", "AlienButNice-Domain-Stack"),
    ];

    internal static readonly AppHostnameExpectation[] AppHostnames =
    [
        new("kapsalon-dev.mikepattyn.nl", "kapsalon"),
        new("kapsalon-acc.mikepattyn.nl", "kapsalon"),
        new("kapsalon.mikepattyn.nl", "kapsalon"),
        new("fish-dev.mikepattyn.nl", "fish"),
        new("fish-acc.mikepattyn.nl", "fish"),
        new("fish.mikepattyn.nl", "fish"),
    ];

    public sealed record BrandHostnameExpectation(string PlatformDomain, string StackId);

    internal static readonly BrandHostnameExpectation[] BrandHostnames =
    [
        new("mikepattyn.nl", "Mikepattyn-BrandFrontend-Stack-Production"),
        new("alienbutnice.nl", "AlienButNice-BrandFrontend-Stack-Production"),
    ];
}
