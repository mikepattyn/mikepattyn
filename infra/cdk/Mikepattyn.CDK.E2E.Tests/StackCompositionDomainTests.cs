using Amazon.CDK;
using Amazon.CDK.Assertions;
using Mikepattyn.CDK;
using Mikepattyn.CDK.Constructs;
using Xunit;

namespace Mikepattyn.CDK.E2E.Tests;

public class StackCompositionDomainTests
{
    [Fact]
    public void Build_ImportsPlatformDomainsWithoutCreatingZonesOrCertificates()
    {
        using var cwd = CdkE2ETestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var composition = StackComposition.Build(app);

        foreach (var expected in ExpectedDomains.PlatformDomains)
        {
            var domainStack = GetPlatformDomainStack(composition, expected.DomainName);
            Assert.Equal(expected.StackId, domainStack.Node.Id);
            Assert.Equal(expected.DomainName, domainStack.DomainName);

            var template = Template.FromStack(domainStack);
            Assert.DoesNotContain("AWS::Route53::HostedZone", template.ToJSON());
            Assert.DoesNotContain("AWS::CertificateManager::Certificate", template.ToJSON());
        }
    }

    [Theory]
    [MemberData(nameof(AppHostnameCases))]
    public void Build_PublishesAppHostnameOnCloudFrontAndRoute53(
        ExpectedDomains.AppHostnameExpectation expected
    )
    {
        using var cwd = CdkE2ETestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var composition = StackComposition.Build(app);

        var stack = GetAppStackForHostname(composition, expected);
        var template = Template.FromStack(stack);

        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["Aliases"] = Match.ArrayWith(new object[] { expected.Fqdn }),
                    }
                ),
            }
        );

        template.HasResourceProperties(
            "AWS::Route53::RecordSet",
            new Dictionary<string, object>
            {
                ["Type"] = "CNAME",
                ["Name"] = $"{expected.Fqdn}.",
            }
        );
    }

    public static IEnumerable<object[]> AppHostnameCases() =>
        ExpectedDomains.AppHostnames.Select(expected => new object[] { expected });

    [Theory]
    [MemberData(nameof(BrandHostnameCases))]
    public void Build_PublishesBrandApexAndWwwOnCloudFrontAndRoute53(
        ExpectedDomains.BrandHostnameExpectation expected
    )
    {
        using var cwd = CdkE2ETestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var composition = StackComposition.Build(app);

        var stack = GetBrandStack(composition, expected.PlatformDomain);
        Assert.Equal(expected.StackId, stack.Node.Id);

        var template = Template.FromStack(stack);
        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["Aliases"] = Match.ArrayWith(
                            new object[] { expected.PlatformDomain, $"www.{expected.PlatformDomain}" }
                        ),
                    }
                ),
            }
        );
        template.ResourceCountIs("AWS::Route53::RecordSet", 4);
        template.HasResourceProperties(
            "AWS::Route53::RecordSet",
            new Dictionary<string, object>
            {
                ["Type"] = "A",
                ["AliasTarget"] = Match.ObjectLike(new Dictionary<string, object>()),
            }
        );
    }

    public static IEnumerable<object[]> BrandHostnameCases() =>
        ExpectedDomains.BrandHostnames.Select(expected => new object[] { expected });

    private static BrandFrontendStack GetBrandStack(
        StackComposition composition,
        string platformDomain
    ) =>
        platformDomain switch
        {
            Constants.Domains.Mikepattyn => composition.MikepattynBrandFrontendStack,
            Constants.Domains.AlienButNice => composition.AlienButNiceBrandFrontendStack,
            _ => throw new ArgumentOutOfRangeException(nameof(platformDomain), platformDomain, null),
        };

    private static DomainStack GetPlatformDomainStack(
        StackComposition composition,
        string domainName
    ) =>
        domainName switch
        {
            Constants.Domains.Mikepattyn => composition.MikepattynDomainStack,
            Constants.Domains.AlienButNice => composition.AlienButNiceDomainStack,
            _ => throw new ArgumentOutOfRangeException(nameof(domainName), domainName, null),
        };

    private static Stack GetAppStackForHostname(
        StackComposition composition,
        ExpectedDomains.AppHostnameExpectation expected
    )
    {
        if (expected.AppSlug == Constants.Apps.KapsalonSlug)
        {
            return composition.KapsalonFrontendStacks.Single(
                stack => stack.DomainName == expected.Fqdn
            );
        }

        if (expected.AppSlug == Constants.Apps.FishSlug)
        {
            return composition.FishEdgeStacks.Single(stack => stack.DomainName == expected.Fqdn);
        }

        if (expected.AppSlug == Constants.Apps.PromptEngineeringSlug)
        {
            return composition.PromptEngineeringFrontendStack.DomainName == expected.Fqdn
                ? composition.PromptEngineeringFrontendStack
                : throw new InvalidOperationException(
                    $"Expected PromptEngineering frontend at {expected.Fqdn}, got {composition.PromptEngineeringFrontendStack.DomainName}."
                );
        }

        throw new ArgumentOutOfRangeException(
            nameof(expected),
            expected.AppSlug,
            "Unsupported application slug."
        );
    }
}
