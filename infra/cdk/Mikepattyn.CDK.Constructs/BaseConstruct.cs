namespace Mikepattyn.CDK.Constructs;

public class BaseConstruct<T> : Construct
    where T : BaseConstructProps
{
    private readonly T ConstructProps;

    public BaseConstruct(Construct scope, T props)
        : base(scope, props.ConstructId)
    {
        ConstructProps = props;
    }

    private string GetAppName(string name) => $"{ConstructProps.AppName}-{name}";

    protected string GetUniqueName(string name) =>
        $"{GetAppName(name)}-{ConstructProps.DeploymentEnvironment.Name}";

    protected string GetUniqueApiName(string name) =>
        $"{GetAppName(name)}-Api-{ConstructProps.DeploymentEnvironment.Name}";
}
