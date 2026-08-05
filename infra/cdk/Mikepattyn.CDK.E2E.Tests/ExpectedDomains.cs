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
        new("barbershop-dev.mikepattyn.nl", "barbershop"),
        new("barbershop-acc.mikepattyn.nl", "barbershop"),
        new("barbershop.mikepattyn.nl", "barbershop"),
        new("gofish-dev.mikepattyn.nl", "gofish"),
        new("gofish-acc.mikepattyn.nl", "gofish"),
        new("gofish.mikepattyn.nl", "gofish"),
        new("prompt-engineering.mikepattyn.nl", "prompt-engineering"),
    ];

    public sealed record BrandHostnameExpectation(string PlatformDomain, string StackId);

    internal static readonly BrandHostnameExpectation[] BrandHostnames =
    [
        new("mikepattyn.nl", "Mikepattyn-BrandFrontend-Stack-Production"),
        new("alienbutnice.nl", "AlienButNice-BrandFrontend-Stack-Production"),
    ];
}
