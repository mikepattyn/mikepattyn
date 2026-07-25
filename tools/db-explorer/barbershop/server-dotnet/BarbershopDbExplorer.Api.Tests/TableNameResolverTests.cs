using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Moq;
using Xunit;

namespace BarbershopDbExplorer.Api.Tests;

public class TableNameResolverTests
{
    [Fact]
    public async Task ResolveTableNameAsync_QueriesExpectedParameterName()
    {
        GetParameterRequest? captured = null;
        var ssm = new Mock<IAmazonSimpleSystemsManagement>();
        ssm.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
            .Callback<GetParameterRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new GetParameterResponse { Parameter = new Parameter { Value = "Kapsalon-Table-Dev" } });

        var resolver = new TableNameResolver();
        var tableName = await resolver.ResolveTableNameAsync(ssm.Object, "dev", profile: null, region: "eu-central-1");

        Assert.Equal("Kapsalon-Table-Dev", tableName);
        Assert.NotNull(captured);
        Assert.Equal("/Kapsalon/Development/Application/DynamoDbTableName", captured!.Name);
    }

    [Fact]
    public async Task ResolveTableNameAsync_CachesByEnvironmentProfileAndRegion()
    {
        var callCount = 0;
        var ssm = new Mock<IAmazonSimpleSystemsManagement>();
        ssm.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
            .Callback(() => callCount++)
            .ReturnsAsync(new GetParameterResponse { Parameter = new Parameter { Value = "Kapsalon-Table-Dev" } });

        var resolver = new TableNameResolver();
        await resolver.ResolveTableNameAsync(ssm.Object, "dev", profile: null, region: "eu-central-1");
        await resolver.ResolveTableNameAsync(ssm.Object, "dev", profile: null, region: "eu-central-1");

        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task ResolveTableNameAsync_DoesNotShareCacheAcrossProfiles()
    {
        var callCount = 0;
        var ssm = new Mock<IAmazonSimpleSystemsManagement>();
        ssm.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
            .Callback(() => callCount++)
            .ReturnsAsync(new GetParameterResponse { Parameter = new Parameter { Value = "Kapsalon-Table-Dev" } });

        var resolver = new TableNameResolver();
        await resolver.ResolveTableNameAsync(ssm.Object, "dev", profile: "profile-a", region: "eu-central-1");
        await resolver.ResolveTableNameAsync(ssm.Object, "dev", profile: "profile-b", region: "eu-central-1");

        Assert.Equal(2, callCount);
    }

    [Fact]
    public async Task ResolveTableNameAsync_ThrowsWhenParameterMissing()
    {
        var ssm = new Mock<IAmazonSimpleSystemsManagement>();
        ssm.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
            .ReturnsAsync(new GetParameterResponse { Parameter = null });

        var resolver = new TableNameResolver();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => resolver.ResolveTableNameAsync(ssm.Object, "dev", profile: null, region: "eu-central-1")
        );
    }
}
