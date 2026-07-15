namespace Mikepattyn.CDK.Constructs;

public abstract class BaseStackProps
{
    protected abstract string StackName { get; }
    public required DeploymentEnvironment DeploymentEnvironment { get; init; }
    public required Amazon.CDK.Environment StackEnvironment { get; init; }

    public string GetUniqueResourceId(string resourceName) =>
        $"{StackName}-{resourceName}-{DeploymentEnvironment.Name}";

    public string StackId =>
        DeploymentEnvironment.Name == DeploymentEnvironment.None.Name
            ? StackName
            : $"{StackName}-{DeploymentEnvironment.Name}";
}
