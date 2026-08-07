namespace Mikepattyn.CDK.Constructs;

public class DashboardBackendStack : BaseStack<DashboardBackendStackProps>
{
    public string ApiUrl { get; }
    public string ApiGatewayHostName { get; }

    public DashboardBackendStack(Construct scope, DashboardBackendStackProps props)
        : base(scope, props)
    {
        var table = new Table(
            this,
            props.GetUniqueResourceId("StatsTable"),
            new TableProps
            {
                TableName = props
                    .GetUniqueResourceId("StatsTable")
                    .Replace("-", "")
                    .ToLower(),
                PartitionKey = new Attribute { Name = "PK", Type = AttributeType.STRING },
                SortKey = new Attribute { Name = "SK", Type = AttributeType.STRING },
                BillingMode = BillingMode.PAY_PER_REQUEST,
                RemovalPolicy = RemovalPolicy.DESTROY,
                TimeToLiveAttribute = "ttl",
            }
        );

        var statsFunction = new Function(
            this,
            props.GetUniqueResourceId("StatsFunction"),
            new FunctionProps
            {
                FunctionName = props.GetUniqueResourceId("StatsFunction"),
                Runtime = Runtime.NODEJS_22_X,
                Handler = "index.handler",
                Code = Code.FromAsset(LambdaAssetPaths.Dashboard),
                Timeout = Duration.Seconds(10),
                MemorySize = 256,
                Environment = new Dictionary<string, string>
                {
                    ["TABLE_NAME"] = table.TableName,
                },
            }
        );

        table.GrantReadWriteData(statsFunction);

        var api = new RestApi(
            this,
            props.GetUniqueResourceId("StatsApi"),
            new RestApiProps
            {
                RestApiName = $"Dashboard.Stats.{props.DeploymentEnvironment.Name}",
                DeployOptions = new StageOptions
                {
                    StageName = props.DeploymentEnvironment.Name,
                    Description = $"Dashboard stats API {props.DeploymentEnvironment.Name}",
                    ThrottlingRateLimit = 100,
                    ThrottlingBurstLimit = 200,
                },
            }
        );

        var apiResource = api.Root.AddResource("api");
        var events = apiResource.AddResource("events");
        events.AddMethod("POST", new LambdaIntegration(statsFunction));

        var stats = apiResource.AddResource("stats");
        stats.AddMethod("GET", new LambdaIntegration(statsFunction));

        var stack = Stack.Of(this);
        ApiGatewayHostName =
            $"{api.RestApiId}.execute-api.{stack.Region}.{stack.UrlSuffix}";
        ApiUrl = $"{api.Url.TrimEnd('/')}/api";

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
                    $"/{Constants.Apps.Dashboard}/{props.DeploymentEnvironment.Name}/Backend/ApiUrl",
            }
        );

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", Constants.Apps.Dashboard));
    }
}
