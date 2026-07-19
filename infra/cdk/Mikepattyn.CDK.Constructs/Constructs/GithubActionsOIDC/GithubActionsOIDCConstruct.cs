namespace Mikepattyn.CDK.Constructs;

public class GithubActionsOIDCConstruct : Construct
{
    public string RoleArn => GithubOIDCRole.RoleArn;

    private Role GithubOIDCRole { get; }

    public GithubActionsOIDCConstruct(
        Construct scope,
        string id,
        GithubActionsOIDCConstructProps props
    )
        : base(scope, id)
    {
        const string githubDomain = "token.actions.githubusercontent.com";
        var stack = Stack.Of(this);

        // Account-global: IAM allows one provider per issuer URL; import rather than create.
        var oidcProvider = OpenIdConnectProvider.FromOpenIdConnectProviderArn(
            this,
            props.GetResourceIdentifier(nameof(OpenIdConnectProvider)),
            props.GithubOidcProviderArn
        );

        // Legacy: repo:owner/repo:* — Immutable (new repos / renames after 2026-07-15):
        // repo:owner@ownerId/repo@repoId:*
        var repositoryParts = props.Repository.Split('/');
        var owner = repositoryParts[0];
        var repoName = repositoryParts[^1];
        var subjectPatterns = new[]
        {
            $"repo:{props.Repository}:*",
            $"repo:{owner}@*/{repoName}@*:*",
        };

        var s3ObjectArns = props.S3BucketArns.Select(arn => $"{arn}/*").ToArray();

        GithubOIDCRole = new Role(
            this,
            props.GetResourceIdentifier(nameof(GithubOIDCRole)),
            new RoleProps
            {
                AssumedBy = new WebIdentityPrincipal(
                    oidcProvider.OpenIdConnectProviderArn,
                    new Dictionary<string, object>
                    {
                        {
                            "StringEquals",
                            new Dictionary<string, string>
                            {
                                { $"{githubDomain}:aud", "sts.amazonaws.com" },
                            }
                        },
                        {
                            "StringLike",
                            new Dictionary<string, object>
                            {
                                { $"{githubDomain}:sub", subjectPatterns },
                            }
                        },
                    }
                ),
                Path = "/github-actions/mikepattyn/",
                RoleName = $"{props.Repository.Split('/').Last()}-frontend-deployment-role",
                InlinePolicies = new Dictionary<string, PolicyDocument>
                {
                    {
                        "frontendDeploy",
                        new PolicyDocument(
                            new PolicyDocumentProps
                            {
                                Statements =
                                [
                                    new PolicyStatement(
                                        new PolicyStatementProps
                                        {
                                            Effect = Effect.ALLOW,
                                            Actions = ["s3:ListBucket"],
                                            Resources = props.S3BucketArns,
                                        }
                                    ),
                                    new PolicyStatement(
                                        new PolicyStatementProps
                                        {
                                            Effect = Effect.ALLOW,
                                            Actions =
                                            [
                                                "s3:GetObject",
                                                "s3:PutObject",
                                                "s3:DeleteObject",
                                            ],
                                            Resources = s3ObjectArns,
                                        }
                                    ),
                                    new PolicyStatement(
                                        new PolicyStatementProps
                                        {
                                            Effect = Effect.ALLOW,
                                            Actions = ["cloudfront:CreateInvalidation"],
                                            Resources = props.CloudFrontDistributionArns,
                                        }
                                    ),
                                    new PolicyStatement(
                                        new PolicyStatementProps
                                        {
                                            Effect = Effect.ALLOW,
                                            Actions = ["ssm:GetParameter", "ssm:GetParameters"],
                                            Resources = props.SsmParameterArns,
                                        }
                                    ),
                                    new PolicyStatement(
                                        new PolicyStatementProps
                                        {
                                            Effect = Effect.ALLOW,
                                            Actions =
                                            [
                                                "lambda:UpdateFunctionCode",
                                                "lambda:GetFunction",
                                            ],
                                            Resources =
                                            [
                                                $"arn:{stack.Partition}:lambda:{stack.Region}:{stack.Account}:function:Kapsalon-*",
                                                $"arn:{stack.Partition}:lambda:{stack.Region}:{stack.Account}:function:Fish-*",
                                            ],
                                        }
                                    ),
                                ],
                            }
                        )
                    },
                },
                Description =
                    "GitHub Actions role for deploying platform apps to S3/CloudFront and backend compute",
                MaxSessionDuration = Duration.Hours(1),
            }
        );
    }
}
