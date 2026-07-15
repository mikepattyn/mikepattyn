namespace Mikepattyn.CDK.Constructs;

public class DynamoDbConstruct : Construct
{
    public Table Application { get; }
    public Key ApplicationEncryptionKey { get; }

    public DynamoDbConstruct(Construct scope, DynamoDbConstructProps props)
        : base(scope, props.ConstructId)
    {
        ApplicationEncryptionKey = new Key(
            this,
            props.GetResourceIdentifier(nameof(ApplicationEncryptionKey)),
            new KeyProps
            {
                Description =
                    $"Customer-managed KMS key for Kapsalon DynamoDB table ({props.DeploymentEnvironment.Name})",
                EnableKeyRotation = true,
                RemovalPolicy = RemovalPolicy.DESTROY,
            }
        );

        Application = new Table(
            this,
            props.GetResourceIdentifier(nameof(Application)),
            new TableProps
            {
                TableName = props.GetTableName(nameof(Application)),
                BillingMode = BillingMode.PAY_PER_REQUEST,
                Encryption = TableEncryption.CUSTOMER_MANAGED,
                EncryptionKey = ApplicationEncryptionKey,
                PartitionKey = new Attribute { Type = AttributeType.STRING, Name = "PK" },
                SortKey = new Attribute { Type = AttributeType.STRING, Name = "SK" },
                RemovalPolicy = RemovalPolicy.DESTROY,
            }
        );

        Application.AddLocalSecondaryIndex(
            new LocalSecondaryIndexProps
            {
                IndexName = "LSI1",
                SortKey = new Attribute { Type = AttributeType.STRING, Name = "SK" },
            }
        );

        Application.AddGlobalSecondaryIndex(
            new GlobalSecondaryIndexProps
            {
                IndexName = "GSI1",
                PartitionKey = new Attribute { Type = AttributeType.STRING, Name = "GSI1PK" },
                SortKey = new Attribute { Type = AttributeType.STRING, Name = "GSI1SK" },
                ProjectionType = ProjectionType.ALL,
            }
        );
    }
}
