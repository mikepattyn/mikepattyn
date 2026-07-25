using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Moq;
using Xunit;

namespace BarbershopDbExplorer.Api.Tests;

public class DynamoQueryOperationsTests
{
    private const string TableName = "Kapsalon-Test";

    [Fact]
    public async Task QueryItemsAsync_UsesBeginsWithWhenNotExact()
    {
        QueryRequest? captured = null;
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.QueryAsync(It.IsAny<QueryRequest>(), default))
            .Callback<QueryRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new QueryResponse { Items = [] });

        await DynamoQueryOperations.QueryItemsAsync(dynamoDb.Object, TableName, "TENANT#x", "STAFF#", exactSk: false);

        Assert.NotNull(captured);
        Assert.Equal("PK = :pk AND begins_with(SK, :skPrefix)", captured!.KeyConditionExpression);
        Assert.Equal("STAFF#", captured.ExpressionAttributeValues[":skPrefix"].S);
    }

    [Fact]
    public async Task QueryItemsAsync_UsesExactSkWhenRequested()
    {
        QueryRequest? captured = null;
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.QueryAsync(It.IsAny<QueryRequest>(), default))
            .Callback<QueryRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new QueryResponse { Items = [] });

        await DynamoQueryOperations.QueryItemsAsync(dynamoDb.Object, TableName, "TENANT#x", "PROFILE", exactSk: true);

        Assert.Equal("PK = :pk AND SK = :sk", captured!.KeyConditionExpression);
        Assert.Equal("PROFILE", captured.ExpressionAttributeValues[":sk"].S);
    }

    [Fact]
    public async Task QueryItemsAsync_FollowsPagination()
    {
        var callCount = 0;
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.QueryAsync(It.IsAny<QueryRequest>(), default))
            .ReturnsAsync(() =>
            {
                callCount++;
                if (callCount == 1)
                {
                    return new QueryResponse
                    {
                        Items = [new() { ["SK"] = new("STAFF#1") }],
                        LastEvaluatedKey = new Dictionary<string, AttributeValue> { ["SK"] = new("STAFF#1") },
                    };
                }

                return new QueryResponse { Items = [new() { ["SK"] = new("STAFF#2") }] };
            });

        var items = await DynamoQueryOperations.QueryItemsAsync(
            dynamoDb.Object,
            TableName,
            "TENANT#x",
            "STAFF#",
            exactSk: false
        );

        Assert.Equal(2, items.Count);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public async Task GetItemAsync_ReturnsNullWhenNotFound()
    {
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(new GetItemResponse { Item = null });

        var result = await DynamoQueryOperations.GetItemAsync(dynamoDb.Object, TableName, "PK1", "SK1");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetItemAsync_ReturnsItemWhenFound()
    {
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(
                new GetItemResponse { Item = new Dictionary<string, AttributeValue> { ["StaffId"] = new("marcus") } }
            );

        var result = await DynamoQueryOperations.GetItemAsync(dynamoDb.Object, TableName, "PK1", "SK1");

        Assert.NotNull(result);
        Assert.Equal("marcus", result!["StaffId"].S);
    }

    [Fact]
    public async Task PutItemAsync_SendsItemToTable()
    {
        PutItemRequest? captured = null;
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.PutItemAsync(It.IsAny<PutItemRequest>(), default))
            .Callback<PutItemRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new PutItemResponse());

        var item = new Dictionary<string, AttributeValue> { ["StaffId"] = new("marcus") };
        await DynamoQueryOperations.PutItemAsync(dynamoDb.Object, TableName, item);

        Assert.NotNull(captured);
        Assert.Equal(TableName, captured!.TableName);
        Assert.Same(item, captured.Item);
    }

    [Fact]
    public async Task DeleteItemAsync_SendsKeyToTable()
    {
        DeleteItemRequest? captured = null;
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), default))
            .Callback<DeleteItemRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new DeleteItemResponse());

        await DynamoQueryOperations.DeleteItemAsync(dynamoDb.Object, TableName, "PK1", "SK1");

        Assert.NotNull(captured);
        Assert.Equal("PK1", captured!.Key["PK"].S);
        Assert.Equal("SK1", captured.Key["SK"].S);
    }

    [Fact]
    public async Task TransactWriteAsync_SendsPutAndDeleteItems()
    {
        TransactWriteItemsRequest? captured = null;
        var dynamoDb = new Mock<IAmazonDynamoDB>();
        dynamoDb
            .Setup(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default))
            .Callback<TransactWriteItemsRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new TransactWriteItemsResponse());

        var putItem = new Dictionary<string, AttributeValue> { ["StaffId"] = new("marcus") };
        var deleteKey = new Dictionary<string, AttributeValue> { ["PK"] = new("x"), ["SK"] = new("y") };

        await DynamoQueryOperations.TransactWriteAsync(
            dynamoDb.Object,
            TableName,
            [
                new TransactWriteItem
                {
                    Put = new Put
                    {
                        Item = putItem,
                        ConditionExpression = "attribute_not_exists(PK)",
                    },
                },
                new TransactWriteItem { Delete = new Delete { Key = deleteKey } },
            ]
        );

        Assert.NotNull(captured);
        Assert.Equal(2, captured!.TransactItems.Count);
        Assert.Equal(TableName, captured.TransactItems[0].Put.TableName);
        Assert.Equal(TableName, captured.TransactItems[1].Delete.TableName);
    }
}
