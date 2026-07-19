using Amazon.CDK;
using Amazon.CDK.Assertions;
using Amazon.CDK.AWS.CertificateManager;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Mikepattyn.CDK.Constructs;
using Xunit;

namespace Mikepattyn.CDK.Constructs.Tests;

public class LambdaConstructTests
{
    [Fact]
    public void LambdaConstruct_CreatesApiAndAuthorizerFunctions()
    {
        using var cwd = CdkTestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var stack = new Stack(app, "TestStack", new StackProps { Env = CdkTestHelpers.TestEnv });
        var dynamoDb = new DynamoDbConstruct(
            stack,
            new DynamoDbConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
            }
        );
        _ = new LambdaConstruct(
            stack,
            new LambdaConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
                DynamoDbConstruct = dynamoDb,
                AuthressApiBasePath = "https://authress.example.com",
                AuthressResourceGroupId = "resource-group",
            }
        );

        var template = Template.FromStack(stack);

        template.ResourceCountIs("AWS::Lambda::Function", 4);
        template.HasResourceProperties(
            "AWS::Lambda::Function",
            new Dictionary<string, object>
            {
                ["Runtime"] = "dotnet10",
                ["Handler"] = "Kapsalon.Scheduling.Api::Kapsalon.Scheduling.Api.Function::FunctionHandler",
            }
        );
    }
}

public class AuthorizersConstructTests
{
    [Fact]
    public void AuthorizersConstruct_CreatesRequestAuthorizer()
    {
        using var cwd = CdkTestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var stack = new Stack(app, "TestStack", new StackProps { Env = CdkTestHelpers.TestEnv });
        var dynamoDb = new DynamoDbConstruct(
            stack,
            new DynamoDbConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
            }
        );
        var lambda = new LambdaConstruct(
            stack,
            new LambdaConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
                DynamoDbConstruct = dynamoDb,
                AuthressApiBasePath = "https://authress.example.com",
                AuthressResourceGroupId = "resource-group",
            }
        );

        var authorizers = new AuthorizersConstruct(
            stack,
            "Authorizers",
            new AuthorizersConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
                AuthorizerFunction = lambda.Authorizer,
            }
        );

        Assert.NotNull(authorizers.RestApiAuthorizer);
    }
}

public class ApiGatewayConstructTests
{
    [Fact]
    public void ApiGatewayConstruct_CreatesRestApiWithStage()
    {
        using var cwd = CdkTestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var stack = new Stack(app, "TestStack", new StackProps { Env = CdkTestHelpers.TestEnv });
        var dynamoDb = new DynamoDbConstruct(
            stack,
            new DynamoDbConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
            }
        );
        var lambda = new LambdaConstruct(
            stack,
            new LambdaConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
                DynamoDbConstruct = dynamoDb,
                AuthressApiBasePath = "https://authress.example.com",
                AuthressResourceGroupId = "resource-group",
            }
        );
        var authorizers = new AuthorizersConstruct(
            stack,
            "Authorizers",
            new AuthorizersConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
                AuthorizerFunction = lambda.Authorizer,
            }
        );

        _ = new ApiGatewayConstruct(
            stack,
            "ApiGateway",
            new ApiGatewayConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
                LambdaConstruct = lambda,
                AuthorizersConstruct = authorizers,
                RateLimitOptions = ApiRateLimitOptions.ForEnvironment(DeploymentEnvironment.Development),
            }
        );

        var template = Template.FromStack(stack);
        template.HasResourceProperties(
            "AWS::ApiGateway::RestApi",
            new Dictionary<string, object> { ["Name"] = "Kapsalon.Api.Development" }
        );
    }
}

public class WebApplicationHostingConstructTests
{
    [Fact]
    public void WebApplicationHostingConstruct_CreatesBucketAndDistribution()
    {
        var app = new App();
        var stack = new Stack(app, "TestStack", new StackProps { Env = CdkTestHelpers.TestEnv });
        _ = new WebApplicationHostingConstruct(
            stack,
            "WebApp",
            new WebApplicationHostingConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DomainNames = AppHostnames.GetDomainNames(
                    Constants.Apps.KapsalonSlug,
                    DeploymentEnvironment.Development,
                    "mikepattyn.nl"
                ),
                DeploymentEnvironment = DeploymentEnvironment.Development,
                Certificate = Certificate.FromCertificateArn(
                    stack,
                    "WebAppCertificate",
                    "arn:aws:acm:us-east-1:123456789012:certificate/00000000-0000-0000-0000-000000000000"
                ),
            }
        );

        var template = Template.FromStack(stack);
        template.ResourceCountIs("AWS::S3::Bucket", 1);
        template.ResourceCountIs("AWS::CloudFront::Distribution", 1);
        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["Aliases"] = Match.ArrayWith(new object[] { "barbershop-dev.mikepattyn.nl" }),
                    }
                ),
            }
        );
    }

    [Fact]
    public void WebApplicationHostingConstruct_WithApiGateway_AddsApiPathBehavior()
    {
        var app = new App();
        var stack = new Stack(app, "TestStack", new StackProps { Env = CdkTestHelpers.TestEnv });
        _ = new WebApplicationHostingConstruct(
            stack,
            "WebAppWithApi",
            new WebApplicationHostingConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DomainNames = AppHostnames.GetDomainNames(
                    Constants.Apps.KapsalonSlug,
                    DeploymentEnvironment.Development,
                    "mikepattyn.nl"
                ),
                DeploymentEnvironment = DeploymentEnvironment.Development,
                Certificate = Certificate.FromCertificateArn(
                    stack,
                    "WebAppApiCertificate",
                    "arn:aws:acm:us-east-1:123456789012:certificate/00000000-0000-0000-0000-000000000000"
                ),
                ApiGatewayDomainName = "abc123.execute-api.eu-central-1.amazonaws.com",
            }
        );

        var template = Template.FromStack(stack);
        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["CacheBehaviors"] = Match.ArrayWith(
                            new object[]
                            {
                                Match.ObjectLike(new Dictionary<string, object> { ["PathPattern"] = "/api/*" }),
                            }
                        ),
                    }
                ),
            }
        );
    }
}

public class StackTests
{
    [Fact]
    public void BackendStack_SynthesizesCoreResources()
    {
        using var cwd = CdkTestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var stack = new BackendStack(app, CdkTestHelpers.CreateBackendStackProps());

        var template = Template.FromStack(stack);
        template.ResourceCountIs("AWS::DynamoDB::Table", 1);
        template.ResourceCountIs("AWS::ApiGateway::RestApi", 1);
    }

    [Fact]
    public void AuthStack_SynthesizesGithubActionsRole()
    {
        var app = new App();
        var stack = new AuthStack(app, CdkTestHelpers.CreateAuthStackProps());

        var template = Template.FromStack(stack);
        template.HasResourceProperties(
            "AWS::IAM::Role",
            new Dictionary<string, object> { ["Path"] = "/github-actions/mikepattyn/" }
        );
        var templateJson = System.Text.Json.JsonSerializer.Serialize(template.ToJSON());
        Assert.DoesNotContain("Custom::AWSCDKOpenIdConnectProvider", templateJson);
        Assert.DoesNotContain("AWS::IAM::OIDCProvider", templateJson);
        Assert.Contains("repo:mikepattyn@*/mikepattyn@*:*", templateJson);
    }

    [Fact]
    public void DomainStack_ImportsHostedZoneAndCertificate()
    {
        var app = new App();
        var stack = new DomainStack(app, CdkTestHelpers.CreateDomainStackProps());

        Assert.Equal(Constants.Domains.Mikepattyn, stack.DomainName);
        Assert.IsType<MikepattynPlatformDomainConstruct>(stack.PlatformDomain);

        var template = Template.FromStack(stack);
        Assert.DoesNotContain("AWS::Route53::HostedZone", template.ToJSON());
        Assert.DoesNotContain("AWS::CertificateManager::Certificate", template.ToJSON());
    }

    [Fact]
    public void DomainStack_AlienButNice_ImportsHostedZoneAndCertificate()
    {
        var app = new App();
        var stack = new DomainStack(app, CdkTestHelpers.CreateAlienButNiceDomainStackProps());

        Assert.Equal(Constants.Domains.AlienButNice, stack.DomainName);
        Assert.IsType<AlienButNicePlatformDomainConstruct>(stack.PlatformDomain);

        var template = Template.FromStack(stack);
        Assert.DoesNotContain("AWS::Route53::HostedZone", template.ToJSON());
        Assert.DoesNotContain("AWS::CertificateManager::Certificate", template.ToJSON());
    }

    [Fact]
    public void FrontendStack_SynthesizesHostingResources()
    {
        var app = new App();
        var importHost = new Stack(app, "ImportHost", new StackProps { Env = CdkTestHelpers.TestEnv });
        var stack = new FrontendStack(app, CdkTestHelpers.CreateFrontendStackProps(importHost));

        var template = Template.FromStack(stack);
        template.ResourceCountIs("AWS::S3::Bucket", 1);
        template.ResourceCountIs("AWS::CloudFront::Distribution", 1);
        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["CacheBehaviors"] = Match.ArrayWith(
                            new object[]
                            {
                                Match.ObjectLike(new Dictionary<string, object> { ["PathPattern"] = "/api/*" }),
                            }
                        ),
                    }
                ),
            }
        );
    }

    [Fact]
    public void BackendStack_PublishesApiUrlWithApiPrefix()
    {
        using var cwd = CdkTestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var stack = new BackendStack(app, CdkTestHelpers.CreateBackendStackProps());

        Assert.EndsWith("/api", stack.ApiUrl);
        Assert.Contains("execute-api", stack.ApiGatewayHostName);

        var template = Template.FromStack(stack);
        template.HasResourceProperties(
            "AWS::SSM::Parameter",
            new Dictionary<string, object> { ["Name"] = "/Kapsalon/Development/Backend/ApiUrl" }
        );
    }

    [Fact]
    public void FishEdgeStack_CreatesPathSplitBehaviors()
    {
        var app = new App();
        var importHost = new Stack(app, "FishImportHost", new StackProps { Env = CdkTestHelpers.TestEnv });
        var stack = new FishEdgeStack(app, CdkTestHelpers.CreateFishEdgeStackProps(importHost));

        var template = Template.FromStack(stack);
        template.ResourceCountIs("AWS::S3::Bucket", 1);
        template.ResourceCountIs("AWS::CloudFront::Distribution", 1);
        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["CacheBehaviors"] = Match.ArrayWith(
                            new object[]
                            {
                                Match.ObjectLike(new Dictionary<string, object> { ["PathPattern"] = "/api/*" }),
                            }
                        ),
                    }
                ),
            }
        );
    }

    [Theory]
    [InlineData(Constants.Apps.Mikepattyn, Constants.Domains.Mikepattyn)]
    [InlineData(Constants.Apps.AlienButNice, Constants.Domains.AlienButNice)]
    public void BrandFrontendStack_SynthesizesApexAndWwwAliasRecords(
        string appName,
        string platformDomain
    )
    {
        var app = new App();
        var importHost = new Stack(app, "BrandImportHost", new StackProps { Env = CdkTestHelpers.TestEnv });
        var stack = new BrandFrontendStack(
            app,
            CdkTestHelpers.CreateBrandFrontendStackProps(importHost, appName, platformDomain)
        );

        var template = Template.FromStack(stack);
        template.ResourceCountIs("AWS::S3::Bucket", 1);
        template.ResourceCountIs("AWS::CloudFront::Distribution", 1);
        template.ResourceCountIs("AWS::Route53::RecordSet", 4);
        template.HasResourceProperties(
            "AWS::CloudFront::Distribution",
            new Dictionary<string, object>
            {
                ["DistributionConfig"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["Aliases"] = Match.ArrayWith(
                            new object[] { platformDomain, $"www.{platformDomain}" }
                        ),
                    }
                ),
            }
        );
        template.HasResourceProperties(
            "AWS::Route53::RecordSet",
            new Dictionary<string, object>
            {
                ["Type"] = "A",
                ["AliasTarget"] = Match.ObjectLike(new Dictionary<string, object>()),
            }
        );
        template.HasResourceProperties(
            "AWS::Route53::RecordSet",
            new Dictionary<string, object>
            {
                ["Type"] = "AAAA",
                ["Name"] = $"www.{platformDomain}.",
            }
        );
    }
}

public class AspectTests
{
    [Fact]
    public void AddEnvironmentVariableToLambdaAspect_AddsEnvironmentVariable()
    {
        using var cwd = CdkTestHelpers.UseCdkWorkingDirectory();
        var app = new App();
        var stack = new Stack(app, "TestStack", new StackProps { Env = CdkTestHelpers.TestEnv });
        _ = new Function(
            stack,
            "TestFunction",
            new FunctionProps
            {
                Runtime = Runtime.DOTNET_10,
                Handler = "test::test.Function::Handler",
                Code = Code.FromAsset(LambdaAssetPaths.KapsalonZip),
            }
        );

        Amazon.CDK.Aspects.Of(stack).Add(new AddEnvironmentVariableToLambdaAspect("TestKey", "TestValue"));

        var template = Template.FromStack(stack);
        template.HasResourceProperties(
            "AWS::Lambda::Function",
            new Dictionary<string, object>
            {
                ["Environment"] = Match.ObjectLike(
                    new Dictionary<string, object>
                    {
                        ["Variables"] = Match.ObjectLike(new Dictionary<string, object> { ["TestKey"] = "TestValue" }),
                    }
                ),
            }
        );
    }

    [Fact]
    public void ImportedOnlyConstructAspect_AllowsImportedDomainResources()
    {
        var app = new App();
        Assert.NotNull(new DomainStack(app, CdkTestHelpers.CreateDomainStackProps()));
    }
}
