using System.Collections.Concurrent;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace BarbershopDbExplorer.Api;

public sealed class TableNameResolver
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public async Task<string> ResolveTableNameAsync(
        IAmazonSimpleSystemsManagement ssm,
        string uiEnv,
        string? profile,
        string region,
        CancellationToken cancellationToken = default
    )
    {
        var deployEnv = DynamoKeys.DeploymentEnv(uiEnv);
        var cacheKey = $"{deployEnv}:{profile ?? "default"}:{region}";

        if (_cache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        var response = await ssm.GetParameterAsync(
            new GetParameterRequest { Name = DynamoKeys.SsmTableParameter(deployEnv) },
            cancellationToken
        );

        var tableName = response.Parameter?.Value;
        if (string.IsNullOrEmpty(tableName))
        {
            throw new InvalidOperationException($"SSM parameter not found for {deployEnv}");
        }

        _cache[cacheKey] = tableName;
        return tableName;
    }
}
