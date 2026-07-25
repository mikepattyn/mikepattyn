using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleSystemsManagement;

namespace BarbershopDbExplorer.Api;

public interface IAwsClientFactory
{
    IAmazonDynamoDB CreateDynamoClient(string? profile, string region);

    IAmazonSimpleSystemsManagement CreateSsmClient(string? profile, string region);
}

[ExcludeFromCodeCoverage]
public sealed class AwsClientFactory : IAwsClientFactory
{
    public IAmazonDynamoDB CreateDynamoClient(string? profile, string region) =>
        new AmazonDynamoDBClient(ResolveCredentials(profile), RegionEndpoint.GetBySystemName(region));

    public IAmazonSimpleSystemsManagement CreateSsmClient(string? profile, string region) =>
        new AmazonSimpleSystemsManagementClient(ResolveCredentials(profile), RegionEndpoint.GetBySystemName(region));

    private static AWSCredentials ResolveCredentials(string? profile)
    {
        if (string.IsNullOrWhiteSpace(profile))
        {
            return FallbackCredentialsFactory.GetCredentials();
        }

        var chain = new CredentialProfileStoreChain();
        if (!chain.TryGetAWSCredentials(profile, out var credentials))
        {
            throw new InvalidOperationException($"AWS profile '{profile}' was not found.");
        }

        return credentials;
    }
}
