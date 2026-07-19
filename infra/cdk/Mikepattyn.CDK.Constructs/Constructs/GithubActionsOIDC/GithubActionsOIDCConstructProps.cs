namespace Mikepattyn.CDK.Constructs;

public class GithubActionsOIDCConstructProps : BaseConstructProps
{
    public required string Repository { get; init; }
    public required string GithubOidcProviderArn { get; init; }
    public required string[] S3BucketArns { get; init; }
    public required string[] CloudFrontDistributionArns { get; init; }
    public required string[] SsmParameterArns { get; init; }
    protected override string ConstructName => "GithubActions";
}
