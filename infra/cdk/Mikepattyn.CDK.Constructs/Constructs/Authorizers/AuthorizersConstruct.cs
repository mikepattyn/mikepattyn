namespace Mikepattyn.CDK.Constructs;

public class AuthorizersConstruct : Construct
{
    public RequestAuthorizer RestApiAuthorizer { get; }

    public AuthorizersConstruct(Construct scope, string id, AuthorizersConstructProps props)
        : base(scope, id)
    {
        RestApiAuthorizer = new RequestAuthorizer(
            this,
            props.GetResourceIdentifier(nameof(RestApiAuthorizer)),
            new RequestAuthorizerProps
            {
                AuthorizerName =
                    $"KapsalonRestApiAuthorizer{props.DeploymentEnvironment.Name}".Replace(" ", ""),
                Handler = props.AuthorizerFunction,
                IdentitySources = [IdentitySource.Header("Authorization")],
            }
        );
    }
}
