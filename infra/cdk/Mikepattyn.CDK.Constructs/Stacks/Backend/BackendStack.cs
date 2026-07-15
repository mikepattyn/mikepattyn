namespace Mikepattyn.CDK.Constructs;

public class BackendStack : BaseStack<BackendStackProps>
{
    private DynamoDbConstruct DynamoDbConstruct { get; }
    private LambdaConstruct LambdaConstruct { get; }
    private ApiGatewayConstruct ApiGatewayConstruct { get; }
    private AuthorizersConstruct AuthorizersConstruct { get; }

    private string ApiUrl => ApiGatewayConstruct.ApiUrl;
    private string ApplicationTableName => DynamoDbConstruct.Application.TableName;

    public BackendStack(Construct scope, BackendStackProps props)
        : base(scope, props)
    {
        DynamoDbConstruct = new DynamoDbConstruct(
            this,
            new DynamoDbConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = props.DeploymentEnvironment,
            }
        );

        LambdaConstruct = new LambdaConstruct(
            this,
            new LambdaConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = props.DeploymentEnvironment,
                DynamoDbConstruct = DynamoDbConstruct,
                AuthressApiBasePath = props.AuthressApiBasePath,
                AuthressResourceGroupId = props.AuthressResourceGroupId,
            }
        );

        AuthorizersConstruct = new AuthorizersConstruct(
            this,
            props.GetUniqueResourceId(nameof(AuthorizersConstruct)),
            new AuthorizersConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = props.DeploymentEnvironment,
                AuthorizerFunction = LambdaConstruct.Authorizer,
            }
        );

        ApiGatewayConstruct = new ApiGatewayConstruct(
            this,
            props.GetUniqueResourceId(nameof(ApiGatewayConstruct)),
            new ApiGatewayConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = props.DeploymentEnvironment,
                LambdaConstruct = LambdaConstruct,
                AuthorizersConstruct = AuthorizersConstruct,
                RateLimitOptions = ApiRateLimitOptions.ForEnvironment(
                    props.DeploymentEnvironment
                ),
            }
        );

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
                    $"/{Constants.Apps.Kapsalon}/{props.DeploymentEnvironment.Name}/Backend/ApiUrl",
            }
        );

        new StringParameter(
            this,
            props.GetUniqueResourceId($"{nameof(ApplicationTableName)}-{nameof(StringParameter)}"),
            new StringParameterProps
            {
                StringValue = ApplicationTableName,
                ParameterName =
                    $"/{Constants.Apps.Kapsalon}/{props.DeploymentEnvironment.Name}/Application/DynamoDbTableName",
            }
        );

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", Constants.Apps.Kapsalon));
        AmazonAspect
            .Of(this)
            .Add(
                new AddEnvironmentVariableToLambdaAspect(
                    "Environment",
                    props.DeploymentEnvironment.Name
                )
            );
        AmazonAspect
            .Of(this)
            .Add(
                new AddEnvironmentVariableToLambdaAspect(
                    "EnvironmentName",
                    props.DeploymentEnvironment.Name
                )
            );
    }
}
