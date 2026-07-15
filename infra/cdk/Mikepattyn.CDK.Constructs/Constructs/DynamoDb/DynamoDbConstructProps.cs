namespace Mikepattyn.CDK.Constructs;

public class DynamoDbConstructProps : BaseConstructProps
{
    public string GetTableName(string tableName) =>
        $"{AppName}-{tableName}-Table-{DeploymentEnvironment.Name}";

    protected override string ConstructName => "DynamoDb";
}
