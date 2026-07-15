using Amazon.CDK;
using Mikepattyn.CDK.Constructs;
using StackEnvironment = Amazon.CDK.Environment;

namespace Mikepattyn.CDK;

sealed class Program
{
    private static readonly string[] FrontendSsmParameterNames =
    [
        "BucketName",
        "DistributionId",
        "CloudFrontDistributionDomain",
        "DomainName",
    ];

    private static readonly string[] BackendSsmParameterNames = ["ApiUrl"];

    public static void Main(string[] args)
    {
        var app = new App();

        var stackEnvironment = new StackEnvironment
        {
            Account = Constants.Deployment.AccountId,
            Region = Constants.Deployment.Region,
        };

        var platformDomain = Constants.Deployment.DomainName;

        var domainResources = new DomainStack(
            app,
            new DomainStackProps
            {
                DeploymentEnvironment = DeploymentEnvironment.None,
                DomainName = platformDomain,
                HostedZoneId = Constants.Deployment.Route53HostedZoneId,
                CertificateArn = Constants.Deployment.CertificateArn,
                StackEnvironment = stackEnvironment,
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
                        HostedZone = domainResources.HostedZone,
                        Certificate = domainResources.Certificate,
                    }
                )
            );
        }

        var fishDataStacks = new List<FishDataStack>();
        var fishApiStacks = new List<FishApiStack>();
        var fishEdgeStacks = new List<FishEdgeStack>();

        foreach (var deploymentEnvironment in deploymentEnvironments)
        {
            var fishData = new FishDataStack(
                app,
                new FishDataStackProps
                {
                    DeploymentEnvironment = deploymentEnvironment,
                    StackEnvironment = stackEnvironment,
                }
            );
            fishDataStacks.Add(fishData);

            var fishApi = new FishApiStack(
                app,
                new FishApiStackProps
                {
                    DeploymentEnvironment = deploymentEnvironment,
                    StackEnvironment = stackEnvironment,
                    Vpc = fishData.Vpc,
                }
            );
            fishApiStacks.Add(fishApi);

            fishEdgeStacks.Add(
                new FishEdgeStack(
                    app,
                    new FishEdgeStackProps
                    {
                        AppSlug = Constants.Apps.FishSlug,
                        DeploymentEnvironment = deploymentEnvironment,
                        PlatformDomainName = platformDomain,
                        StackEnvironment = stackEnvironment,
                        HostedZone = domainResources.HostedZone,
                        Certificate = domainResources.Certificate,
                        LoadBalancerDnsName = fishApi.LoadBalancerDnsName,
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
                deploymentEnvironments.Select(
                    deploymentEnvironment =>
                        $"arn:aws:ssm:{Constants.Deployment.Region}:{Constants.Deployment.AccountId}:parameter/{Constants.Apps.Kapsalon}/{deploymentEnvironment.Name}/Backend/ApiUrl"
                )
            )
            .ToArray();

        new AuthStack(
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
            new BackendStack(
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

        app.Synth();
    }
}
