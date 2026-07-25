using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace BarbershopDbExplorer.Api;

public static class DynamoQueryOperations
{
    public static async Task<List<Dictionary<string, AttributeValue>>> QueryItemsAsync(
        IAmazonDynamoDB dynamoDb,
        string tableName,
        string pk,
        string skPrefix,
        bool exactSk,
        CancellationToken cancellationToken = default
    )
    {
        var items = new List<Dictionary<string, AttributeValue>>();
        Dictionary<string, AttributeValue>? lastKey = null;

        do
        {
            var response = await dynamoDb.QueryAsync(
                new QueryRequest
                {
                    TableName = tableName,
                    KeyConditionExpression = exactSk
                        ? "PK = :pk AND SK = :sk"
                        : "PK = :pk AND begins_with(SK, :skPrefix)",
                    ExpressionAttributeValues = exactSk
                        ? new Dictionary<string, AttributeValue> { [":pk"] = new(pk), [":sk"] = new(skPrefix) }
                        : new Dictionary<string, AttributeValue>
                        {
                            [":pk"] = new(pk),
                            [":skPrefix"] = new(skPrefix),
                        },
                    ExclusiveStartKey = lastKey,
                },
                cancellationToken
            );

            items.AddRange(response.Items);
            lastKey = response.LastEvaluatedKey is { Count: > 0 } ? response.LastEvaluatedKey : null;
        } while (lastKey is not null);

        return items;
    }

    public static async Task<Dictionary<string, AttributeValue>?> GetItemAsync(
        IAmazonDynamoDB dynamoDb,
        string tableName,
        string pk,
        string sk,
        CancellationToken cancellationToken = default
    )
    {
        var response = await dynamoDb.GetItemAsync(
            new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue> { ["PK"] = new(pk), ["SK"] = new(sk) },
            },
            cancellationToken
        );

        return response.Item is { Count: > 0 } ? response.Item : null;
    }

    public static Task PutItemAsync(
        IAmazonDynamoDB dynamoDb,
        string tableName,
        Dictionary<string, AttributeValue> item,
        CancellationToken cancellationToken = default
    ) => dynamoDb.PutItemAsync(new PutItemRequest { TableName = tableName, Item = item }, cancellationToken);

    public static Task DeleteItemAsync(
        IAmazonDynamoDB dynamoDb,
        string tableName,
        string pk,
        string sk,
        CancellationToken cancellationToken = default
    ) =>
        dynamoDb.DeleteItemAsync(
            new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue> { ["PK"] = new(pk), ["SK"] = new(sk) },
            },
            cancellationToken
        );

    public static Task TransactWriteAsync(
        IAmazonDynamoDB dynamoDb,
        string tableName,
        IReadOnlyList<TransactWriteItem> items,
        CancellationToken cancellationToken = default
    )
    {
        foreach (var item in items)
        {
            if (item.Put is not null)
            {
                item.Put.TableName = tableName;
            }

            if (item.Delete is not null)
            {
                item.Delete.TableName = tableName;
            }
        }

        return dynamoDb.TransactWriteItemsAsync(
            new TransactWriteItemsRequest { TransactItems = items.ToList() },
            cancellationToken
        );
    }
}
