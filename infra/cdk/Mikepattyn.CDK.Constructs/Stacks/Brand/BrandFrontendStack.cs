using Amazon.CDK.AWS.Route53.Targets;

namespace Mikepattyn.CDK.Constructs;

public class BrandFrontendStack : BaseStack<BrandFrontendStackProps>
{
    private WebApplicationHostingConstruct WebApp { get; }

    public string BucketName => WebApp.S3Bucket.BucketName;
    public string BucketArn => WebApp.S3Bucket.BucketArn;
    public string DistributionId => WebApp.Distribution.DistributionId;
    public string DistributionArn => WebApp.Distribution.DistributionArn;
    public string DomainName { get; }

    public BrandFrontendStack(Construct scope, BrandFrontendStackProps props)
        : base(scope, props)
    {
        DomainName = BrandHostnames.Primary(props.PlatformDomainName);
        var domainNames = BrandHostnames.GetDomainNames(props.PlatformDomainName);

        WebApp = new WebApplicationHostingConstruct(
            this,
            props.GetUniqueResourceId(nameof(WebApp)),
            new WebApplicationHostingConstructProps
            {
                AppName = props.AppName,
                DomainNames = domainNames,
                Certificate = props.Certificate,
                DeploymentEnvironment = props.DeploymentEnvironment,
            }
        );

        var cloudFrontTarget = new CloudFrontTarget(WebApp.Distribution);

        CreateAliasRecord(
            this,
            props,
            props.GetUniqueResourceId("ApexA"),
            recordName: string.Empty,
            cloudFrontTarget
        );
        CreateAliasRecord(
            this,
            props,
            props.GetUniqueResourceId("ApexAAAA"),
            recordName: string.Empty,
            cloudFrontTarget,
            ipv6: true
        );
        CreateAliasRecord(
            this,
            props,
            props.GetUniqueResourceId("WwwA"),
            recordName: "www",
            cloudFrontTarget
        );
        CreateAliasRecord(
            this,
            props,
            props.GetUniqueResourceId("WwwAAAA"),
            recordName: "www",
            cloudFrontTarget,
            ipv6: true
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

    private static void CreateAliasRecord(
        Construct scope,
        BrandFrontendStackProps props,
        string id,
        string recordName,
        CloudFrontTarget target,
        bool ipv6 = false
    )
    {
        var aliasTarget = RecordTarget.FromAlias(target);

        if (ipv6)
        {
            new AaaaRecord(
                scope,
                id,
                new AaaaRecordProps
                {
                    Zone = props.HostedZone,
                    RecordName = recordName,
                    Target = aliasTarget,
                }
            );
            return;
        }

        new ARecord(
            scope,
            id,
            new ARecordProps
            {
                Zone = props.HostedZone,
                RecordName = recordName,
                Target = aliasTarget,
            }
        );
    }

    private void PublishOutput(BrandFrontendStackProps props, string name, string value)
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

    private void PublishSsmParameter(BrandFrontendStackProps props, string name, string value)
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
