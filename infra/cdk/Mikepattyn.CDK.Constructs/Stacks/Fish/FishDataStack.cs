namespace Mikepattyn.CDK.Constructs;

public class FishDataStack : BaseStack<FishDataStackProps>
{
    public Bucket PhotosBucket { get; }
    public DatabaseInstance Database { get; }
    public Vpc Vpc { get; }

    public FishDataStack(Construct scope, FishDataStackProps props)
        : base(scope, props)
    {
        Vpc = new Vpc(
            this,
            props.GetUniqueResourceId(nameof(Vpc)),
            new VpcProps
            {
                MaxAzs = 2,
                NatGateways = 1,
            }
        );

        PhotosBucket = new Bucket(
            this,
            props.GetUniqueResourceId(nameof(PhotosBucket)),
            new BucketProps
            {
                BucketName = props
                    .GetUniqueResourceId(nameof(PhotosBucket))
                    .Replace("-", "")
                    .ToLower(),
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                Encryption = BucketEncryption.S3_MANAGED,
                EnforceSSL = true,
                RemovalPolicy = RemovalPolicy.DESTROY,
                AutoDeleteObjects = true,
            }
        );

        var databaseCredentials = new Amazon.CDK.AWS.SecretsManager.Secret(
            this,
            props.GetUniqueResourceId("DatabaseCredentials"),
            new SecretProps
            {
                SecretName = $"Fish/Database/{props.DeploymentEnvironment.Name}",
                GenerateSecretString = new SecretStringGenerator
                {
                    SecretStringTemplate = "{\"username\":\"fishadmin\"}",
                    GenerateStringKey = "password",
                    ExcludePunctuation = true,
                },
            }
        );

        Database = new DatabaseInstance(
            this,
            props.GetUniqueResourceId(nameof(Database)),
            new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.Postgres(
                    new PostgresInstanceEngineProps { Version = PostgresEngineVersion.VER_16 }
                ),
                InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.T3, InstanceSize.MICRO),
                Vpc = Vpc,
                VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PRIVATE_WITH_EGRESS },
                Credentials = Credentials.FromSecret(databaseCredentials),
                DatabaseName = "fishtracking",
                AllocatedStorage = 20,
                RemovalPolicy = RemovalPolicy.DESTROY,
                DeletionProtection = false,
                MultiAz = false,
            }
        );

        new StringParameter(
            this,
            props.GetUniqueResourceId("PhotosBucketName"),
            new StringParameterProps
            {
                ParameterName =
                    $"/{Constants.Apps.Fish}/{props.DeploymentEnvironment.Name}/Data/PhotosBucketName",
                StringValue = PhotosBucket.BucketName,
            }
        );

        AmazonAspect
            .Of(this)
            .Add(new Amazon.CDK.Tag("Environment", props.DeploymentEnvironment.Name));
        AmazonAspect.Of(this).Add(new Amazon.CDK.Tag("App", Constants.Apps.Fish));
    }
}
