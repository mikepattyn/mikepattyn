namespace Mikepattyn.CDK.Constructs;

public class FishApiGatewayConstruct : Construct
{
    public RestApi Api { get; }
    public string ApiUrl => Api.Url;

    public FishApiGatewayConstruct(Construct scope, string id, FishApiGatewayConstructProps props)
        : base(scope, id)
    {
        var rateLimitOptions = props.RateLimitOptions;

        Api = new RestApi(
            this,
            props.GetResourceIdentifier(nameof(Api)),
            new RestApiProps
            {
                RestApiName = $"Fish.Api.{props.DeploymentEnvironment.Name}",
                DeployOptions = new StageOptions
                {
                    StageName = props.DeploymentEnvironment.Name,
                    Description = $"Deployment for Fish {props.DeploymentEnvironment.Name}",
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

        var authorizedMethodOptions = new MethodOptions
        {
            AuthorizationType = AuthorizationType.CUSTOM,
            Authorizer = props.AuthorizersConstruct.RestApiAuthorizer,
        };

        var api = Api.Root.AddResource("api");
        var health = api.AddResource("health");
        health.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Profile),
            new MethodOptions { AuthorizationType = AuthorizationType.NONE }
        );

        var spots = api.AddResource(
            "spots",
            new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions }
        );
        spots.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Spots));
        var nearby = spots.AddResource("nearby");
        nearby.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Spots));
        var discover = spots.AddResource("discover");
        discover.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Spots));
        var spot = spots.AddResource("{spotId}");
        spot.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Spots));
        var checkin = spot.AddResource("checkin");
        checkin.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Spots));

        var catches = api.AddResource(
            "catches",
            new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions }
        );
        catches.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Catches));
        var catchNearby = catches.AddResource("nearby");
        catchNearby.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Catches));
        var analyze = catches.AddResource("analyze");
        analyze.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Catches));

        var profile = api.AddResource(
            "profile",
            new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions }
        );
        profile.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Profile));
        var fog = profile.AddResource("fog");
        fog.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Profile));

        var missions = api.AddResource("missions");
        var daily = missions.AddResource("daily");
        daily.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Profile), authorizedMethodOptions);

        var community = api.AddResource(
            "community",
            new ResourceOptions { DefaultMethodOptions = authorizedMethodOptions }
        );
        var communitySpots = community.AddResource("spots");
        var communitySpot = communitySpots.AddResource("{spotId}");
        var comments = communitySpot.AddResource("comments");
        comments.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Community));
        comments.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Community));
        var rate = communitySpot.AddResource("rate");
        rate.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Community));
        var competition = community.AddResource("competition");
        competition.AddMethod("GET", new LambdaIntegration(props.LambdaConstruct.Community));

        var events = api.AddResource("events");
        events.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Community),
            authorizedMethodOptions
        );

        var routes = api.AddResource("routes");
        routes.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Community),
            authorizedMethodOptions
        );
        var route = routes.AddResource("{routeId}");
        var complete = route.AddResource("complete");
        complete.AddMethod("POST", new LambdaIntegration(props.LambdaConstruct.Community), authorizedMethodOptions);

        var premium = api.AddResource("premium");
        var catalog = premium.AddResource("catalog");
        catalog.AddMethod(
            "GET",
            new LambdaIntegration(props.LambdaConstruct.Community),
            new MethodOptions { AuthorizationType = AuthorizationType.NONE }
        );
    }
}

public class FishApiGatewayConstructProps : BaseConstructProps
{
    public required FishLambdaConstruct LambdaConstruct { get; init; }
    public required AuthorizersConstruct AuthorizersConstruct { get; init; }
    public required ApiRateLimitOptions RateLimitOptions { get; init; }
    protected override string ConstructName => "ApiGateway";
}
