namespace Mikepattyn.CDK.Constructs;

public class WebApplicationHostingConstruct : Construct
{
    public Bucket S3Bucket { get; }
    public Distribution Distribution { get; }
    private S3OriginAccessControl OriginAccessControl { get; }

    public WebApplicationHostingConstruct(
        Construct scope,
        string id,
        WebApplicationHostingConstructProps props
    )
        : base(scope, id)
    {
        S3Bucket = CreateS3Bucket(props);
        CreateS3BucketPolicy();
        OriginAccessControl = CreateOriginAccessControl(props);
        Distribution = CreateDistribution(props);
    }

    private S3OriginAccessControl CreateOriginAccessControl(
        WebApplicationHostingConstructProps props
    )
    {
        return new S3OriginAccessControl(
            this,
            props.GetResourceIdentifier(nameof(OriginAccessControl)),
            new S3OriginAccessControlProps
            {
                OriginAccessControlName = props
                    .GetResourceIdentifier(nameof(OriginAccessControl))
                    .Replace("-", "")
                    .ToLower(),
                Description =
                    $"Origin Access Control that allows CloudFront to access the S3 {props.DeploymentEnvironment.Name} bucket for {props.AppName} securely",
                Signing = new Signing(SigningProtocol.SIGV4, SigningBehavior.ALWAYS),
            }
        );
    }

    private Distribution CreateDistribution(WebApplicationHostingConstructProps props)
    {
        var hostname = AppHostnames.For(
            props.AppSlug,
            props.DeploymentEnvironment,
            props.PlatformDomainName
        );

        return new Distribution(
            this,
            props.GetResourceIdentifier(nameof(Distribution)),
            new DistributionProps
            {
                DefaultBehavior = new BehaviorOptions
                {
                    Origin = S3BucketOrigin.WithOriginAccessControl(
                        S3Bucket,
                        new S3BucketOriginWithOACProps { OriginAccessControl = OriginAccessControl }
                    ),
                    ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS,
                    Compress = true,
                    AllowedMethods = AllowedMethods.ALLOW_GET_HEAD_OPTIONS,
                    CachedMethods = CachedMethods.CACHE_GET_HEAD_OPTIONS,
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
                Comment =
                    $"CloudFront distribution for serving {hostname} in {props.DeploymentEnvironment.Name} environment",
                PriceClass = PriceClass.PRICE_CLASS_100,
                DomainNames = [hostname],
                Certificate = props.Certificate,
            }
        );
    }

    private void CreateS3BucketPolicy()
    {
        S3Bucket.AddToResourcePolicy(
            new PolicyStatement(
                new PolicyStatementProps
                {
                    Effect = Effect.DENY,
                    Principals = [new AnyPrincipal()],
                    Actions = ["s3:*"],
                    Resources = [S3Bucket.BucketArn, $"{S3Bucket.BucketArn}/*"],
                    Conditions = new Dictionary<string, object>
                    {
                        ["Bool"] = new Dictionary<string, object>
                        {
                            ["aws:SecureTransport"] = "false",
                        },
                    },
                }
            )
        );
    }

    private Bucket CreateS3Bucket(WebApplicationHostingConstructProps props)
    {
        return new Bucket(
            this,
            props.GetResourceIdentifier(nameof(Bucket)),
            new BucketProps
            {
                BucketName = props.GetResourceIdentifier(nameof(Bucket)).Replace("-", "").ToLower(),
                Versioned = false,
                RemovalPolicy = RemovalPolicy.DESTROY,
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                Encryption = BucketEncryption.S3_MANAGED,
                EnforceSSL = true,
                ObjectOwnership = ObjectOwnership.BUCKET_OWNER_ENFORCED,
                Cors =
                [
                    new CorsRule
                    {
                        AllowedMethods = [HttpMethods.GET, HttpMethods.HEAD],
                        AllowedOrigins = ["*"],
                        AllowedHeaders = ["*"],
                        MaxAge = 3000,
                    },
                ],
            }
        );
    }
}
