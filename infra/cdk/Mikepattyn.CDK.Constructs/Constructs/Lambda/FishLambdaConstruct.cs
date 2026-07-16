namespace Mikepattyn.CDK.Constructs;

public interface IFishRestApiFunctions
{
    Function Spots { get; }
    Function Catches { get; }
    Function Profile { get; }
    Function Community { get; }
}

public class FishLambdaConstruct
    : BaseConstruct<FishLambdaConstructProps>,
        IFishRestApiFunctions,
        RequestAuthorizerFunction
{
    public Function Spots { get; }
    public Function Catches { get; }
    public Function Profile { get; }
    public Function Community { get; }
    public Function Authorizer { get; }

    public FishLambdaConstruct(Construct scope, FishLambdaConstructProps props)
        : base(scope, props)
    {
        ArgumentNullException.ThrowIfNull(props.DynamoDbConstruct);

        var authressSecret = new Amazon.CDK.AWS.SecretsManager.Secret(
            this,
            props.GetResourceIdentifier("AuthressServiceClientAccessKey"),
            new SecretProps
            {
                SecretName = $"Fish/Authress/ServiceClientAccessKey/{props.DeploymentEnvironment.Name}",
                Description = "Authress service client access key for Fish API Lambdas",
            }
        );

        var authressEnv = BuildAuthressEnvironment(props, authressSecret);
        var gsiArn = $"{props.DynamoDbConstruct.Application.TableArn}/index/GSI1";

        Spots = CreateApiFunction(
            props,
            nameof(Spots),
            "Fish.Spots.Api::Fish.Spots.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query", "dynamodb:UpdateItem", "dynamodb:TransactWriteItems"],
            [props.DynamoDbConstruct.Application.TableArn, gsiArn],
            authressEnv,
            props.PhotosBucketName
        );

        Catches = CreateApiFunction(
            props,
            nameof(Catches),
            "Fish.Catches.Api::Fish.Catches.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query", "dynamodb:UpdateItem", "s3:PutObject"],
            [props.DynamoDbConstruct.Application.TableArn, gsiArn, $"{props.PhotosBucketArn}/*"],
            authressEnv,
            props.PhotosBucketName
        );

        Profile = CreateApiFunction(
            props,
            nameof(Profile),
            "Fish.Profile.Api::Fish.Profile.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query", "dynamodb:UpdateItem"],
            [props.DynamoDbConstruct.Application.TableArn, gsiArn],
            authressEnv,
            props.PhotosBucketName
        );

        Community = CreateApiFunction(
            props,
            nameof(Community),
            "Fish.Community.Api::Fish.Community.Api.Function::FunctionHandler",
            ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query", "dynamodb:UpdateItem"],
            [props.DynamoDbConstruct.Application.TableArn, gsiArn],
            authressEnv,
            props.PhotosBucketName
        );

        foreach (var function in new[] { Spots, Catches, Profile, Community })
        {
            props.DynamoDbConstruct.Application.GrantReadWriteData(function);
            props.DynamoDbConstruct.ApplicationEncryptionKey.GrantDecrypt(function);
        }

        props.PhotosBucket.GrantPut(Catches);

        Authorizer = new Function(
            this,
            props.GetResourceIdentifier(nameof(Authorizer)),
            new FunctionProps
            {
                FunctionName = GetUniqueName(nameof(Authorizer)),
                Handler = "Fish.Auth::Fish.Auth.Function::FunctionHandler",
                Code = Code.FromAsset(LambdaAssetPaths.FishZip),
                Runtime = Runtime.DOTNET_10,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                Environment = authressEnv,
            }
        );
        authressSecret.GrantRead(Authorizer);
    }

    private static Dictionary<string, string> BuildAuthressEnvironment(
        FishLambdaConstructProps props,
        Amazon.CDK.AWS.SecretsManager.Secret authressSecret
    ) =>
        new()
        {
            { "AuthressApiBasePath", props.AuthressApiBasePath },
            { "AuthressResourceGroupId", props.AuthressResourceGroupId },
            { "AuthressServiceClientAccessKeySecretArn", authressSecret.SecretArn },
        };

    private Function CreateApiFunction(
        FishLambdaConstructProps props,
        string name,
        string handler,
        string[] actions,
        string[] resources,
        Dictionary<string, string> extraEnv,
        string photosBucketName
    )
    {
        var environment = new Dictionary<string, string>(extraEnv)
        {
            { "TableName", props.DynamoDbConstruct.Application.TableName },
            { "PhotosBucketName", photosBucketName },
        };

        return new Function(
            this,
            props.GetResourceIdentifier(name),
            new FunctionProps
            {
                FunctionName = GetUniqueApiName(name),
                Handler = handler,
                Code = Code.FromAsset(LambdaAssetPaths.FishZip),
                Runtime = Runtime.DOTNET_10,
                Timeout = Duration.Seconds(30),
                MemorySize = 256,
                Environment = environment,
                InitialPolicy =
                [
                    new PolicyStatement(
                        new PolicyStatementProps
                        {
                            Actions = actions,
                            Resources = resources,
                        }
                    ),
                ],
            }
        );
    }
}

public class FishLambdaConstructProps : BaseConstructProps
{
    protected override string ConstructName => "Lambda";
    public required DynamoDbConstruct DynamoDbConstruct { get; init; }
    public required string AuthressApiBasePath { get; init; }
    public required string AuthressResourceGroupId { get; init; }
    public required Bucket PhotosBucket { get; init; }
    public required string PhotosBucketName { get; init; }
    public required string PhotosBucketArn { get; init; }
}
