using Amazon.CDK;
using Amazon.CDK.AWS.CertificateManager;
using Amazon.CDK.AWS.Route53;

namespace Mikepattyn.CDK.Constructs.Tests;

internal static class CdkTestHelpers
{
    internal static Amazon.CDK.Environment TestEnv { get; } =
        new() { Account = "123456789012", Region = "eu-central-1" };

    internal static BackendStackProps CreateBackendStackProps() =>
        new()
        {
            DeploymentEnvironment = DeploymentEnvironment.Development,
            StackEnvironment = TestEnv,
            AuthressApiBasePath = "https://authress.example.com",
            AuthressResourceGroupId = "resource-group-id",
        };

    internal static AuthStackProps CreateAuthStackProps() =>
        new()
        {
            DeploymentEnvironment = DeploymentEnvironment.None,
            StackEnvironment = TestEnv,
            Repository = "mikepattyn/mikepattyn",
            S3BucketArns = ["arn:aws:s3:::kapsalon-frontend-dev"],
            CloudFrontDistributionArns = ["arn:aws:cloudfront::123456789012:distribution/ABC123"],
            SsmParameterArns =
            [
                "arn:aws:ssm:eu-central-1:123456789012:parameter/Kapsalon/Development/Backend/ApiUrl",
            ],
        };

    internal static DomainStackProps CreateDomainStackProps() =>
        new()
        {
            DeploymentEnvironment = DeploymentEnvironment.None,
            StackEnvironment = TestEnv,
            DomainName = "mikepattyn.nl",
            HostedZoneId = "Z1234567890ABC",
            CertificateArn =
                "arn:aws:acm:us-east-1:123456789012:certificate/00000000-0000-0000-0000-000000000000",
        };

    internal static FrontendStackProps CreateFrontendStackProps(Stack stack) =>
        new()
        {
            AppName = Constants.Apps.Kapsalon,
            AppSlug = Constants.Apps.KapsalonSlug,
            DeploymentEnvironment = DeploymentEnvironment.Development,
            StackEnvironment = TestEnv,
            PlatformDomainName = "mikepattyn.nl",
            HostedZone = HostedZone.FromHostedZoneAttributes(
                stack,
                "ImportedHostedZone",
                new HostedZoneAttributes
                {
                    HostedZoneId = "Z1234567890ABC",
                    ZoneName = "mikepattyn.nl",
                }
            ),
            Certificate = Certificate.FromCertificateArn(
                stack,
                "ImportedCertificate",
                "arn:aws:acm:us-east-1:123456789012:certificate/00000000-0000-0000-0000-000000000000"
            ),
        };

    internal static FishEdgeStackProps CreateFishEdgeStackProps(Stack stack) =>
        new()
        {
            AppSlug = Constants.Apps.FishSlug,
            DeploymentEnvironment = DeploymentEnvironment.Development,
            StackEnvironment = TestEnv,
            PlatformDomainName = "mikepattyn.nl",
            LoadBalancerDnsName = "fish-alb.example.com",
            HostedZone = HostedZone.FromHostedZoneAttributes(
                stack,
                "FishImportedHostedZone",
                new HostedZoneAttributes
                {
                    HostedZoneId = "Z1234567890ABC",
                    ZoneName = "mikepattyn.nl",
                }
            ),
            Certificate = Certificate.FromCertificateArn(
                stack,
                "FishImportedCertificate",
                "arn:aws:acm:us-east-1:123456789012:certificate/00000000-0000-0000-0000-000000000000"
            ),
        };

    internal static IDisposable UseCdkWorkingDirectory()
    {
        var previous = Directory.GetCurrentDirectory();
        var repoRoot = FindRepoRoot();
        Directory.SetCurrentDirectory(Path.Combine(repoRoot, "infra", "cdk"));
        return new RestoreWorkingDirectory(previous);
    }

    private sealed class RestoreWorkingDirectory(string previous) : IDisposable
    {
        public void Dispose() => Directory.SetCurrentDirectory(previous);
    }

    private static string FindRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "Makefile")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }
}
