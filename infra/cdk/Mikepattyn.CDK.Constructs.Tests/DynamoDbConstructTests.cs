using Amazon.CDK;
using Amazon.CDK.Assertions;
using Amazon.CDK.AWS.DynamoDB;
using Mikepattyn.CDK.Constructs;
using Xunit;

namespace Mikepattyn.CDK.Constructs.Tests;

public class DynamoDbConstructTests
{
    [Fact]
    public void ApplicationTable_DefinesPkSkAndIndexes()
    {
        var app = new App();
        var stack = new Stack(app, "TestStack");
        _ = new DynamoDbConstruct(
            stack,
            new DynamoDbConstructProps
            {
                AppName = Constants.Apps.Kapsalon,
                DeploymentEnvironment = DeploymentEnvironment.Development,
            }
        );

        var template = Template.FromStack(stack);

        template.HasResourceProperties(
            "AWS::DynamoDB::Table",
            new Dictionary<string, object>
            {
                ["BillingMode"] = "PAY_PER_REQUEST",
                ["KeySchema"] = Match.ArrayWith(
                    [
                        new Dictionary<string, string> { ["AttributeName"] = "PK", ["KeyType"] = "HASH" },
                        new Dictionary<string, string> { ["AttributeName"] = "SK", ["KeyType"] = "RANGE" },
                    ]
                ),
                ["GlobalSecondaryIndexes"] = Match.ArrayWith(
                    [
                        Match.ObjectLike(
                            new Dictionary<string, object>
                            {
                                ["IndexName"] = "GSI1",
                                ["KeySchema"] = Match.ArrayWith(
                                    [
                                        new Dictionary<string, string>
                                        {
                                            ["AttributeName"] = "GSI1PK",
                                            ["KeyType"] = "HASH",
                                        },
                                        new Dictionary<string, string>
                                        {
                                            ["AttributeName"] = "GSI1SK",
                                            ["KeyType"] = "RANGE",
                                        },
                                    ]
                                ),
                            }
                        ),
                    ]
                ),
                ["LocalSecondaryIndexes"] = Match.ArrayWith(
                    [Match.ObjectLike(new Dictionary<string, object> { ["IndexName"] = "LSI1" })]
                ),
            }
        );
    }
}
