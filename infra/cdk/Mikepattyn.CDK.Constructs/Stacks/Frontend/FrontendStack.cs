using Amazon.CDK.AWS.Route53.Targets;

namespace Mikepattyn.CDK.Constructs;

public class FrontendStack : BaseStack<FrontendStackProps>
{
    private WebApplicationHostingConstruct WebApp { get; }

    public string BucketName => WebApp.S3Bucket.BucketName;
    public string BucketArn => WebApp.S3Bucket.BucketArn;
    public string DistributionId => WebApp.Distribution.DistributionId;
    public string DistributionArn => WebApp.Distribution.DistributionArn;
    public string DomainName { get; }

    public FrontendStack(Construct scope, FrontendStackProps props)
        : base(scope, props)
    {
        DomainName = AppHostnames.For(
            props.AppSlug,
            props.DeploymentEnvironment,
            props.PlatformDomainName
        );

        WebApp = new WebApplicationHostingConstruct(
            this,
            props.GetUniqueResourceId(nameof(WebApp)),
            new WebApplicationHostingConstructProps
            {
                AppName = props.AppName,
                DomainNames = AppHostnames.GetDomainNames(
                    props.AppSlug,
                    props.DeploymentEnvironment,
                    props.PlatformDomainName
                ),
                Certificate = props.Certificate,
                DeploymentEnvironment = props.DeploymentEnvironment,
                ApiGatewayDomainName = props.ApiGatewayDomainName,
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
                DomainName = WebApp.Distribution.DomainName,
                Ttl = Duration.Seconds(300),
            }
        );

        PublishOutput(props, nameof(BucketName), BucketName);
        PublishOutput(props, nameof(DistributionId), DistributionId);
        PublishOutput(
            props,
            "CloudFrontDistributionDomain",
            WebApp.Distribution.DomainName
        );
        PublishOutput(props, nameof(DomainName), DomainName);

        PublishSsmParameter(props, nameof(BucketName), BucketName);
        PublishSsmParameter(props, nameof(DistributionId), DistributionId);
        PublishSsmParameter(
            props,
            "CloudFrontDistributionDomain",
            WebApp.Distribution.DomainName
        );
        PublishSsmParameter(props, nameof(DomainName), DomainName);

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", props.AppName));
    }

    private void PublishOutput(FrontendStackProps props, string name, string value)
    {
        new CfnOutput(
            this,
            props.GetUniqueResourceId($"{name}-{nameof(CfnOutput)}"),
            new CfnOutputProps
            {
                ExportName = GetCfnOutputExportName(name),
                Value = value,
            }
        );
    }

    private void PublishSsmParameter(FrontendStackProps props, string name, string value)
    {
        new StringParameter(
            this,
            props.GetUniqueResourceId($"{name}-{nameof(StringParameter)}"),
            new StringParameterProps
            {
                StringValue = value,
                ParameterName = $"/{props.AppName}/{props.DeploymentEnvironment.Name}/Frontend/{name}",
            }
        );
    }
}
