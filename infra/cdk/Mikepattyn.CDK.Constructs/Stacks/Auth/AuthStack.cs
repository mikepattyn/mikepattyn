namespace Mikepattyn.CDK.Constructs;

public class AuthStack : Stack
{
    private GithubActionsOIDCConstruct GithubActionsOIDC { get; }
    public string GithubActionsRoleArn => GithubActionsOIDC.RoleArn;

    public AuthStack(Construct scope, AuthStackProps props)
        : base(scope, props.StackId, new StackProps { Env = props.StackEnvironment })
    {
        GithubActionsOIDC = new GithubActionsOIDCConstruct(
            this,
            props.GetUniqueResourceId(nameof(GithubActionsOIDC)),
            new GithubActionsOIDCConstructProps
            {
                AppName = Constants.PlatformName,
                Repository = props.Repository,
                S3BucketArns = props.S3BucketArns,
                CloudFrontDistributionArns = props.CloudFrontDistributionArns,
                SsmParameterArns = props.SsmParameterArns,
                DeploymentEnvironment = props.DeploymentEnvironment,
            }
        );

        new CfnOutput(
            this,
            props.GetUniqueResourceId($"{nameof(GithubActionsRoleArn)}-{nameof(CfnOutput)}"),
            new CfnOutputProps
            {
                ExportName = $"{Constants.PlatformName}{nameof(GithubActionsRoleArn)}",
                Value = GithubActionsRoleArn,
            }
        );
    }
}
