namespace Mikepattyn.CDK.Constructs;

public class FishApiStack : BaseStack<FishApiStackProps>
{
    public ApplicationLoadBalancedFargateService Service { get; }
    public string LoadBalancerDnsName => Service.LoadBalancer.LoadBalancerDnsName;

    public FishApiStack(Construct scope, FishApiStackProps props)
        : base(scope, props)
    {
        var cluster = new Cluster(
            this,
            props.GetUniqueResourceId(nameof(Cluster)),
            new ClusterProps { Vpc = props.Vpc }
        );

        Service = new ApplicationLoadBalancedFargateService(
            this,
            props.GetUniqueResourceId(nameof(Service)),
            new ApplicationLoadBalancedFargateServiceProps
            {
                Cluster = cluster,
                Cpu = 256,
                MemoryLimitMiB = 512,
                DesiredCount = 1,
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = ContainerImage.FromRegistry("public.ecr.aws/nginx/nginx:stable"),
                    ContainerPort = 80,
                },
                PublicLoadBalancer = true,
                AssignPublicIp = true,
                TaskSubnets = new SubnetSelection { SubnetType = SubnetType.PUBLIC },
            }
        );

        Service.TargetGroup.ConfigureHealthCheck(
            new Amazon.CDK.AWS.ElasticLoadBalancingV2.HealthCheck
            {
                Path = "/",
                HealthyHttpCodes = "200-399",
            }
        );

        Service.LoadBalancer.SetAttribute("idle_timeout.timeout_seconds", "120");

        Service.TargetGroup.EnableCookieStickiness(Duration.Hours(1));

        new StringParameter(
            this,
            props.GetUniqueResourceId("LoadBalancerDnsName"),
            new StringParameterProps
            {
                ParameterName =
                    $"/{Constants.Apps.Fish}/{props.DeploymentEnvironment.Name}/Api/LoadBalancerDnsName",
                StringValue = LoadBalancerDnsName,
            }
        );

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", Constants.Apps.Fish));
    }
}
