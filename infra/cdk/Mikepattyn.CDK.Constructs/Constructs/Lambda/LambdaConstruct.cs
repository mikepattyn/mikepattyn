namespace Mikepattyn.CDK.Constructs;

public interface IRestApiFunctions
{
    Function Scheduling { get; }
    Function Identity { get; }
    Function Tenant { get; }
}

public interface RequestAuthorizerFunction
{
    Function Authorizer { get; }
}

public class LambdaConstruct
    : BaseConstruct<LambdaConstructProps>,
        IRestApiFunctions,
        RequestAuthorizerFunction
{
    public Function Scheduling { get; }
    public Function Identity { get; }
    public Function Tenant { get; }
    public Function Authorizer { get; }

    public LambdaConstruct(Construct scope, LambdaConstructProps props)
        : base(scope, props)
    {
        ArgumentNullException.ThrowIfNull(props.DynamoDbConstruct);

        var authressSecret = new Amazon.CDK.AWS.SecretsManager.Secret(
            this,
            props.GetResourceIdentifier("AuthressServiceClientAccessKey"),
            new SecretProps
            {
                SecretName = $"Kapsalon/Authress/ServiceClientAccessKey/{props.DeploymentEnvironment.Name}",
                Description = "Authress service client access key for API and authorizer Lambdas",
            }
        );

        var authressEnv = BuildAuthressEnvironment(props, authressSecret);
        var gsiArn = $"{props.DynamoDbConstruct.Application.TableArn}/index/GSI1";

        Scheduling = CreateApiFunction(
            props,
            nameof(Scheduling),
            "Kapsalon.Scheduling.Api::Kapsalon.Scheduling.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query", "dynamodb:UpdateItem"],
            [props.DynamoDbConstruct.Application.TableArn, gsiArn],
            authressEnv
        );
        props.DynamoDbConstruct.Application.GrantReadWriteData(Scheduling);
        props.DynamoDbConstruct.ApplicationEncryptionKey.GrantDecrypt(Scheduling);

        Identity = CreateApiFunction(
            props,
            nameof(Identity),
            "Kapsalon.Identity.Api::Kapsalon.Identity.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query"],
            [props.DynamoDbConstruct.Application.TableArn],
            authressEnv
        );
        props.DynamoDbConstruct.Application.GrantReadWriteData(Identity);
        props.DynamoDbConstruct.ApplicationEncryptionKey.GrantDecrypt(Identity);
        authressSecret.GrantRead(Identity);

        Tenant = CreateApiFunction(
            props,
            nameof(Tenant),
            "Kapsalon.Tenant.Api::Kapsalon.Tenant.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query"],
            [props.DynamoDbConstruct.Application.TableArn],
            []
        );
        props.DynamoDbConstruct.Application.GrantReadWriteData(Tenant);
        props.DynamoDbConstruct.ApplicationEncryptionKey.GrantDecrypt(Tenant);

        Authorizer = new Function(
            this,
            props.GetResourceIdentifier(nameof(Authorizer)),
            new FunctionProps
            {
                FunctionName = GetUniqueName(nameof(Authorizer)),
                Handler = "Kapsalon.Auth::Kapsalon.Auth.Function::FunctionHandler",
                Code = Code.FromAsset(LambdaAssetPaths.KapsalonZip),
                Runtime = Runtime.DOTNET_10,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                InitialPolicy =
                [
                    new PolicyStatement(
                        new PolicyStatementProps
                        {
                            Actions = ["cognito-idp:ListUsers"],
                            Resources = ["*"],
                        }
                    ),
                ],
                Environment = authressEnv,
            }
        );
        authressSecret.GrantRead(Authorizer);
    }

    private static Dictionary<string, string> BuildAuthressEnvironment(
        LambdaConstructProps props,
        Amazon.CDK.AWS.SecretsManager.Secret authressSecret
    ) =>
        new()
        {
            { "AuthressApiBasePath", props.AuthressApiBasePath },
            { "AuthressResourceGroupId", props.AuthressResourceGroupId },
            { "AuthressServiceClientAccessKeySecretArn", authressSecret.SecretArn },
        };

    private Function CreateApiFunction(
        LambdaConstructProps props,
        string name,
        string handler,
        string[] dynamoActions,
        string[] dynamoResources,
        Dictionary<string, string> extraEnv
    )
    {
        var environment = new Dictionary<string, string>(extraEnv)
        {
            { "TableName", props.DynamoDbConstruct.Application.TableName },
        };

        var function = new Function(
            this,
            props.GetResourceIdentifier(name),
            new FunctionProps
            {
                FunctionName = GetUniqueApiName(name),
                Handler = handler,
                Code = Code.FromAsset(LambdaAssetPaths.KapsalonZip),
                Runtime = Runtime.DOTNET_10,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                Environment = environment,
                InitialPolicy =
                [
                    new PolicyStatement(
                        new PolicyStatementProps
                        {
                            Actions = dynamoActions,
                            Resources = dynamoResources,
                        }
                    ),
                ],
            }
        );

        return function;
    }
}
