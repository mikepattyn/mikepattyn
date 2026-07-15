namespace Mikepattyn.CDK.Constructs;

public class AuthorizersConstructProps : BaseConstructProps
{
    protected override string ConstructName => "Authorizers";
    public required Function AuthorizerFunction { get; init; }
}
