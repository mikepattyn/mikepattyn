using Amazon.CDK.AWS.Route53.Targets;

namespace Mikepattyn.CDK.Constructs;

public class FishEdgeStack : BaseStack<FishEdgeStackProps>
{
    public Bucket WebBucket { get; }
    public Distribution Distribution { get; }
    public string DomainName { get; }

    public FishEdgeStack(Construct scope, FishEdgeStackProps props)
        : base(scope, props)
    {
        DomainName = AppHostnames.For(
            props.AppSlug,
            props.DeploymentEnvironment,
            props.PlatformDomainName
        );

        WebBucket = new Bucket(
            this,
            props.GetUniqueResourceId(nameof(WebBucket)),
            new BucketProps
            {
                BucketName = props.GetUniqueResourceId(nameof(WebBucket)).Replace("-", "").ToLower(),
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                Encryption = BucketEncryption.S3_MANAGED,
                EnforceSSL = true,
                RemovalPolicy = RemovalPolicy.DESTROY,
                AutoDeleteObjects = true,
            }
        );

        var originAccessControl = new S3OriginAccessControl(
            this,
            props.GetUniqueResourceId(nameof(S3OriginAccessControl)),
            new S3OriginAccessControlProps
            {
                OriginAccessControlName = props
                    .GetUniqueResourceId("oac")
                    .Replace("-", "")
                    .ToLower(),
                Signing = new Signing(SigningProtocol.SIGV4, SigningBehavior.ALWAYS),
            }
        );

        var spaOrigin = S3BucketOrigin.WithOriginAccessControl(
            WebBucket,
            new S3BucketOriginWithOACProps { OriginAccessControl = originAccessControl }
        );

        var apiOrigin = new HttpOrigin(props.ApiGatewayDomainName, new HttpOriginProps
        {
            OriginPath = $"/{props.DeploymentEnvironment.Name}",
        });

        Distribution = new Distribution(
            this,
            props.GetUniqueResourceId(nameof(Distribution)),
            new DistributionProps
            {
                DefaultBehavior = new BehaviorOptions
                {
                    Origin = spaOrigin,
                    ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS,
                    Compress = true,
                    AllowedMethods = AllowedMethods.ALLOW_GET_HEAD_OPTIONS,
                    CachedMethods = CachedMethods.CACHE_GET_HEAD_OPTIONS,
                },
                AdditionalBehaviors = new Dictionary<string, IBehaviorOptions>
                {
                    ["/api/*"] = new BehaviorOptions
                    {
                        Origin = apiOrigin,
                        ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS,
                        AllowedMethods = AllowedMethods.ALLOW_ALL,
                        CachePolicy = CachePolicy.CACHING_DISABLED,
                        OriginRequestPolicy = OriginRequestPolicy.ALL_VIEWER,
                    },
                },
                DefaultRootObject = "index.html",
                ErrorResponses =
                [
                    new ErrorResponse
                    {
                        HttpStatus = 403,
                        ResponseHttpStatus = 200,
                        ResponsePagePath = "/index.html",
                        Ttl = Duration.Seconds(0),
                    },
                    new ErrorResponse
                    {
                        HttpStatus = 404,
                        ResponseHttpStatus = 200,
                        ResponsePagePath = "/index.html",
                        Ttl = Duration.Seconds(0),
                    },
                ],
                DomainNames = [DomainName],
                Certificate = props.Certificate,
                Comment =
                    $"Fish edge distribution for {DomainName} ({props.DeploymentEnvironment.Name})",
            }
        );

        new CnameRecord(
            this,
            props.GetUniqueResourceId(nameof(CnameRecord)),
            new CnameRecordProps
            {
                Zone = props.HostedZone,
                RecordName = AppHostnames.GetRecordName(
                    props.AppSlug,
                    props.DeploymentEnvironment
                ),
                DomainName = Distribution.DomainName,
                Ttl = Duration.Seconds(300),
            }
        );

        PublishSsm(props, nameof(WebBucket), WebBucket.BucketName);
        PublishSsm(props, "DistributionId", Distribution.DistributionId);
        PublishSsm(props, nameof(DomainName), DomainName);

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", Constants.Apps.Fish));
    }

    private void PublishSsm(FishEdgeStackProps props, string name, string value)
    {
        new StringParameter(
            this,
            props.GetUniqueResourceId($"{name}-Ssm"),
            new StringParameterProps
            {
                ParameterName =
                    $"/{Constants.Apps.Fish}/{props.DeploymentEnvironment.Name}/Frontend/{name}",
                StringValue = value,
            }
        );
    }
}
