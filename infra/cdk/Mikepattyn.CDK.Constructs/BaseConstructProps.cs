namespace Mikepattyn.CDK.Constructs;

public abstract class BaseConstructProps
{
    protected abstract string ConstructName { get; }
    public required string AppName { get; init; }
    public required DeploymentEnvironment DeploymentEnvironment { get; init; }

    public virtual string GetResourceIdentifier(string resource) =>
        $"{ConstructName}-{resource}-{DeploymentEnvironment.Name}";

    public string ConstructId => $"{ConstructName}-Construct-{DeploymentEnvironment.Name}";
}
