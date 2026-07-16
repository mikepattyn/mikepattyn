using Amazon.CDK;
using Mikepattyn.CDK.Constructs;
using StackEnvironment = Amazon.CDK.Environment;

namespace Mikepattyn.CDK;

/// <summary>
/// Wires the full DeployApp stack graph. Shared by <see cref="Program"/> and synth e2e tests.
/// </summary>
public sealed class StackComposition
{
    private static readonly string[] FrontendSsmParameterNames =
    [
        "BucketName",
        "DistributionId",
        "CloudFrontDistributionDomain",
        "DomainName",
    ];

    public DomainStack MikepattynDomainStack { get; }
    public DomainStack AlienButNiceDomainStack { get; }
    public IReadOnlyList<FrontendStack> KapsalonFrontendStacks { get; }
    public IReadOnlyList<FishEdgeStack> FishEdgeStacks { get; }

    private StackComposition(
        DomainStack mikepattynDomainStack,
        DomainStack alienButNiceDomainStack,
        IReadOnlyList<FrontendStack> kapsalonFrontendStacks,
        IReadOnlyList<FishEdgeStack> fishEdgeStacks
    )
    {
        MikepattynDomainStack = mikepattynDomainStack;
        AlienButNiceDomainStack = alienButNiceDomainStack;
        KapsalonFrontendStacks = kapsalonFrontendStacks;
        FishEdgeStacks = fishEdgeStacks;
    }

    public static StackComposition Build(App app)
    {
        var stackEnvironment = new StackEnvironment
        {
            Account = Constants.Deployment.AccountId,
            Region = Constants.Deployment.Region,
        };

        var platformDomain = Constants.Deployment.DomainName;

        var mikepattynDomainStack = new DomainStack(
            app,
            new DomainStackProps
            {
                DeploymentEnvironment = DeploymentEnvironment.None,
                PlatformStackName = Constants.Stacks.Domain,
                HostedZoneId = Constants.Deployment.Route53HostedZoneId,
                CertificateArn = Constants.Deployment.CertificateArn,
                StackEnvironment = stackEnvironment,
                CreatePlatformDomain = static (scope, id, props) =>
                    new MikepattynPlatformDomainConstruct(scope, id, props),
            }
        );

        var alienButNiceDomainStack = new DomainStack(
            app,
            new DomainStackProps
            {
                DeploymentEnvironment = DeploymentEnvironment.None,
                PlatformStackName = Constants.Stacks.AlienButNiceDomain,
                HostedZoneId = Constants.Deployment.AlienButNiceRoute53HostedZoneId,
                CertificateArn = Constants.Deployment.AlienButNiceCertificateArn,
                StackEnvironment = stackEnvironment,
                CreatePlatformDomain = static (scope, id, props) =>
                    new AlienButNicePlatformDomainConstruct(scope, id, props),
            }
        );

        var deploymentEnvironments = new[]
        {
            DeploymentEnvironment.Development,
            DeploymentEnvironment.Staging,
            DeploymentEnvironment.Production,
        };

        var kapsalonFrontendStacks = new List<FrontendStack>();
        foreach (var deploymentEnvironment in deploymentEnvironments)
        {
            kapsalonFrontendStacks.Add(
                new FrontendStack(
                    app,
                    new FrontendStackProps
                    {
                        AppName = Constants.Apps.Kapsalon,
                        AppSlug = Constants.Apps.KapsalonSlug,
                        DeploymentEnvironment = deploymentEnvironment,
                        PlatformDomainName = platformDomain,
                        StackEnvironment = stackEnvironment,
                        HostedZone = mikepattynDomainStack.HostedZone,
                        Certificate = mikepattynDomainStack.Certificate,
                    }
                )
            );
        }

        var fishEdgeStacks = new List<FishEdgeStack>();

        foreach (var deploymentEnvironment in deploymentEnvironments)
        {
            var fishBackend = new FishBackendStack(
                app,
                new FishBackendStackProps
                {
                    DeploymentEnvironment = deploymentEnvironment,
                    StackEnvironment = stackEnvironment,
                    AuthressApiBasePath = Constants.Deployment.AuthressApiBasePath,
                    AuthressResourceGroupId = Constants.Deployment.AuthressResourceGroupId,
                }
            );

            fishEdgeStacks.Add(
                new FishEdgeStack(
                    app,
                    new FishEdgeStackProps
                    {
                        AppSlug = Constants.Apps.FishSlug,
                        DeploymentEnvironment = deploymentEnvironment,
                        PlatformDomainName = platformDomain,
                        StackEnvironment = stackEnvironment,
                        HostedZone = mikepattynDomainStack.HostedZone,
                        Certificate = mikepattynDomainStack.Certificate,
                        ApiGatewayDomainName = fishBackend.ApiGatewayHostName,
                    }
                )
            );
        }

        var ssmParameterArns = deploymentEnvironments
            .SelectMany(
                deploymentEnvironment =>
                    new[] { Constants.Apps.Kapsalon, Constants.Apps.Fish }.SelectMany(
                        appName =>
                            FrontendSsmParameterNames.Select(
                                parameterName =>
                                    $"arn:aws:ssm:{Constants.Deployment.Region}:{Constants.Deployment.AccountId}:parameter/{appName}/{deploymentEnvironment.Name}/Frontend/{parameterName}"
                            )
                    )
            )
            .Concat(
                deploymentEnvironments.SelectMany(
                    deploymentEnvironment =>
                        new[]
                        {
                            $"arn:aws:ssm:{Constants.Deployment.Region}:{Constants.Deployment.AccountId}:parameter/{Constants.Apps.Kapsalon}/{deploymentEnvironment.Name}/Backend/ApiUrl",
                            $"arn:aws:ssm:{Constants.Deployment.Region}:{Constants.Deployment.AccountId}:parameter/{Constants.Apps.Fish}/{deploymentEnvironment.Name}/Backend/ApiUrl",
                        }
                )
            )
            .ToArray();

        _ = new AuthStack(
            app,
            new AuthStackProps
            {
                DeploymentEnvironment = DeploymentEnvironment.None,
                StackEnvironment = stackEnvironment,
                Repository = Constants.Deployment.GithubRepository,
                S3BucketArns = kapsalonFrontendStacks
                    .Select(stack => stack.BucketArn)
                    .Concat(fishEdgeStacks.Select(stack => stack.WebBucket.BucketArn))
                    .ToArray(),
                CloudFrontDistributionArns = kapsalonFrontendStacks
                    .Select(stack => stack.DistributionArn)
                    .Concat(fishEdgeStacks.Select(stack => stack.Distribution.DistributionArn))
                    .ToArray(),
                SsmParameterArns = ssmParameterArns,
            }
        );

        foreach (var deploymentEnvironment in deploymentEnvironments)
        {
            _ = new BackendStack(
                app,
                new BackendStackProps
                {
                    DeploymentEnvironment = deploymentEnvironment,
                    StackEnvironment = stackEnvironment,
                    AuthressApiBasePath = Constants.Deployment.AuthressApiBasePath,
                    AuthressResourceGroupId = Constants.Deployment.AuthressResourceGroupId,
                }
            );
        }

        return new StackComposition(
            mikepattynDomainStack,
            alienButNiceDomainStack,
            kapsalonFrontendStacks,
            fishEdgeStacks
        );
    }
}
