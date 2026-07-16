namespace Mikepattyn.CDK.Constructs;

public class FishBackendStackProps : BaseStackProps
{
    public required string AuthressApiBasePath { get; init; }
    public required string AuthressResourceGroupId { get; init; }
    protected override string StackName => Constants.Stacks.FishBackend;
}

public class FishBackendStack : BaseStack<FishBackendStackProps>
{
    public string ApiUrl { get; }
    public string ApiGatewayHostName { get; }
    public string PhotosBucketName { get; }

    public FishBackendStack(Construct scope, FishBackendStackProps props)
        : base(scope, props)
    {
        var dynamoDbConstruct = new DynamoDbConstruct(
            this,
            new DynamoDbConstructProps
            {
                AppName = Constants.Apps.Fish,
                DeploymentEnvironment = props.DeploymentEnvironment,
            }
        );

        var photosBucket = new Bucket(
            this,
            props.GetUniqueResourceId(nameof(Bucket)),
            new BucketProps
            {
                BucketName = props
                    .GetUniqueResourceId("Photos")
                    .Replace("-", "")
                    .ToLower(),
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                Encryption = BucketEncryption.S3_MANAGED,
                EnforceSSL = true,
                RemovalPolicy = RemovalPolicy.DESTROY,
                AutoDeleteObjects = true,
            }
        );

        PhotosBucketName = photosBucket.BucketName;

        var lambdaConstruct = new FishLambdaConstruct(
            this,
            new FishLambdaConstructProps
            {
                AppName = Constants.Apps.Fish,
                DeploymentEnvironment = props.DeploymentEnvironment,
                DynamoDbConstruct = dynamoDbConstruct,
                AuthressApiBasePath = props.AuthressApiBasePath,
                AuthressResourceGroupId = props.AuthressResourceGroupId,
                PhotosBucket = photosBucket,
                PhotosBucketName = photosBucket.BucketName,
                PhotosBucketArn = photosBucket.BucketArn,
            }
        );

        var authorizersConstruct = new AuthorizersConstruct(
            this,
            props.GetUniqueResourceId(nameof(AuthorizersConstruct)),
            new AuthorizersConstructProps
            {
                AppName = Constants.Apps.Fish,
                DeploymentEnvironment = props.DeploymentEnvironment,
                AuthorizerFunction = lambdaConstruct.Authorizer,
            }
        );

        var apiGatewayConstruct = new FishApiGatewayConstruct(
            this,
            props.GetUniqueResourceId(nameof(FishApiGatewayConstruct)),
            new FishApiGatewayConstructProps
            {
                AppName = Constants.Apps.Fish,
                DeploymentEnvironment = props.DeploymentEnvironment,
                LambdaConstruct = lambdaConstruct,
                AuthorizersConstruct = authorizersConstruct,
                RateLimitOptions = ApiRateLimitOptions.ForEnvironment(props.DeploymentEnvironment),
            }
        );

        ApiUrl = apiGatewayConstruct.ApiUrl;
        var stack = Stack.Of(this);
        ApiGatewayHostName =
            $"{apiGatewayConstruct.Api.RestApiId}.execute-api.{stack.Region}.{stack.UrlSuffix}";

        new CfnOutput(
            this,
            props.GetUniqueResourceId($"{nameof(ApiUrl)}-{nameof(CfnOutput)}"),
            new CfnOutputProps
            {
                ExportName = GetCfnOutputExportName(nameof(ApiUrl)),
                Value = ApiUrl,
            }
        );

        new StringParameter(
            this,
            props.GetUniqueResourceId($"{nameof(ApiUrl)}-{nameof(StringParameter)}"),
            new StringParameterProps
            {
                StringValue = ApiUrl,
                ParameterName =
                    $"/{Constants.Apps.Fish}/{props.DeploymentEnvironment.Name}/Backend/ApiUrl",
            }
        );

        new StringParameter(
            this,
            props.GetUniqueResourceId("PhotosBucketName"),
            new StringParameterProps
            {
                StringValue = photosBucket.BucketName,
                ParameterName =
                    $"/{Constants.Apps.Fish}/{props.DeploymentEnvironment.Name}/Data/PhotosBucketName",
            }
        );

        new StringParameter(
            this,
            props.GetUniqueResourceId("DynamoDbTableName"),
            new StringParameterProps
            {
                StringValue = dynamoDbConstruct.Application.TableName,
                ParameterName =
                    $"/{Constants.Apps.Fish}/{props.DeploymentEnvironment.Name}/Application/DynamoDbTableName",
            }
        );

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", Constants.Apps.Fish));
    }
}
