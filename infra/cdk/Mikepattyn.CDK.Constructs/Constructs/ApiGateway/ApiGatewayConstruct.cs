namespace Mikepattyn.CDK.Constructs;

public class ApiGatewayConstruct : Construct
{
    public RestApi Api { get; }
    public string ApiUrl => Api.Url;

    public ApiGatewayConstruct(Construct scope, string id, ApiGatewayConstructProps props)
        : base(scope, id)
    {
        var rateLimitOptions = props.RateLimitOptions;

        Api = new RestApi(
            this,
            props.GetResourceIdentifier(nameof(Api)),
            new RestApiProps
            {
                RestApiName = $"Kapsalon.Api.{props.DeploymentEnvironment.Name}",
                DeployOptions = new StageOptions
                {
                    StageName = props.DeploymentEnvironment.Name,
                    Description = $"Deployment for Kapsalon {props.DeploymentEnvironment.Name}",
                    ThrottlingRateLimit = rateLimitOptions.ThrottlingRateLimit,
                    ThrottlingBurstLimit = rateLimitOptions.ThrottlingBurstLimit,
                },
                DefaultCorsPreflightOptions = new CorsOptions
                {
                    AllowOrigins = Cors.ALL_ORIGINS,
                    AllowMethods = Cors.ALL_METHODS,
                    AllowHeaders = ["Content-Type", "Authorization"],
                },
            }
        );

        var webAcl = CreateWebAcl(props);
        var stack = Stack.Of(this);
        new CfnWebACLAssociation(
            this,
            props.GetResourceIdentifier("WebAclAssociation"),
            new CfnWebACLAssociationProps
            {
                ResourceArn =
                    $"arn:{stack.Partition}:apigateway:{stack.Region}::/restapis/{Api.RestApiId}/stages/{Api.DeploymentStage.StageName}",
                WebAclArn = webAcl.AttrArn,
            }
        );

        // Add CORS headers to gateway-level error responses so that auth failures
        // and throttled requests surface as real HTTP errors instead of being
        // masked as CORS failures in the browser.
        Api.AddGatewayResponse(
            props.GetResourceIdentifier("Default4XX"),
            new GatewayResponseOptions
            {
                Type = ResponseType_.DEFAULT_4XX,
                ResponseHeaders = new Dictionary<string, string>
                {
                    ["Access-Control-Allow-Origin"] = "'*'",
                    ["Access-Control-Allow-Headers"] = "'Content-Type,Authorization'",
                },
            }
        );

        Api.AddGatewayResponse(
            props.GetResourceIdentifier("Throttled"),
            new GatewayResponseOptions
            {
                Type = ResponseType_.THROTTLED,
                ResponseHeaders = new Dictionary<string, string>
                {
                    ["Access-Control-Allow-Origin"] = "'*'",
                    ["Access-Control-Allow-Headers"] = "'Content-Type,Authorization'",
                },
            }
        );

        Api.AddGatewayResponse(
            props.GetResourceIdentifier("Default5XX"),
            new GatewayResponseOptions
            {
                Type = ResponseType_.DEFAULT_5XX,
                ResponseHeaders = new Dictionary<string, string>
                {
                    ["Access-Control-Allow-Origin"] = "'*'",
                    ["Access-Control-Allow-Headers"] = "'Content-Type,Authorization'",
                },
            }
        );

        var authorizedMethodOptions = new MethodOptions
        {
            AuthorizationType = AuthorizationType.CUSTOM,
            Authorizer = props.AuthorizersConstruct.RestApiAuthorizer,
        };

        var me = Api.Root.AddResource("me", new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions });
        var myAppointments = me.AddResource("appointments");
        myAppointments.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Scheduling));

        var tenants = Api.Root.AddResource("tenants");
        var tenant = tenants.AddResource("{tenantId}");

        var tenantIdentity = tenant.AddResource(
            "identity",
            new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions }
        );
        var invites = tenantIdentity.AddResource("invites");
        invites.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Identity));

        var scheduling = tenant.AddResource(
            "scheduling",
            new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions }
        );
        var appointments = scheduling.AddResource("appointments");
        appointments.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Scheduling));
        appointments.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Scheduling));

        var availability = scheduling.AddResource("availability");
        var availabilityPreload = availability.AddResource("preload");
        availabilityPreload.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Scheduling),
            new MethodOptions { AuthorizationType = AuthorizationType.NONE }
        );
        availability.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Scheduling),
            new MethodOptions { AuthorizationType = AuthorizationType.NONE }
        );

        var walkIns = scheduling.AddResource("walk-ins");
        walkIns.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Scheduling));

        var identity = Api.Root.AddResource("identity");
        var login = identity.AddResource("login");
        login.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Identity));

        var register = identity.AddResource("register");
        register.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Identity));

        var identityMe = identity.AddResource("me");
        identityMe.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Identity),
            authorizedMethodOptions
        );
    }

    private CfnWebACL CreateWebAcl(ApiGatewayConstructProps props) =>
        new(
            this,
            props.GetResourceIdentifier("WebAcl"),
            new CfnWebACLProps
            {
                Name = props.GetResourceIdentifier("WebAcl"),
                Description =
                    $"Rate-based protection for Kapsalon API {props.DeploymentEnvironment.Name}",
                Scope = "REGIONAL",
                DefaultAction = new CfnWebACL.DefaultActionProperty
                {
                    Allow = new CfnWebACL.AllowActionProperty(),
                },
                VisibilityConfig = BuildVisibilityConfig(
                    $"KapsalonApi{props.DeploymentEnvironment.Name}WebAcl"
                ),
                Rules = new object[]
                {
                    new CfnWebACL.RuleProperty
                    {
                        Name = "PerIpRateLimit",
                        Priority = 0,
                        Action = new CfnWebACL.RuleActionProperty
                        {
                            Block = new CfnWebACL.BlockActionProperty(),
                        },
                        Statement = new CfnWebACL.StatementProperty
                        {
                            RateBasedStatement = new CfnWebACL.RateBasedStatementProperty
                            {
                                AggregateKeyType = "IP",
                                Limit = props.RateLimitOptions.WafRateLimit,
                                EvaluationWindowSec =
                                    props.RateLimitOptions.WafEvaluationWindowSeconds,
                            },
                        },
                        VisibilityConfig = BuildVisibilityConfig(
                            $"KapsalonApi{props.DeploymentEnvironment.Name}PerIpRateLimit"
                        ),
                    },
                },
            }
        );

    private static CfnWebACL.VisibilityConfigProperty BuildVisibilityConfig(
        string metricName
    ) =>
        new()
        {
            CloudWatchMetricsEnabled = true,
            MetricName = metricName,
            SampledRequestsEnabled = true,
        };
}
