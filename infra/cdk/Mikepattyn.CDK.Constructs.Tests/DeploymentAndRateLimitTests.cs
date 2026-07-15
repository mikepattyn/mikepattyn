using Mikepattyn.CDK.Constructs;
using Xunit;

namespace Mikepattyn.CDK.Constructs.Tests;

public class AppHostnamesTests
{
    private const string PlatformDomain = "mikepattyn.nl";

    [Theory]
    [InlineData("kapsalon", "Development", "kapsalon-dev.mikepattyn.nl")]
    [InlineData("kapsalon", "Staging", "kapsalon-acc.mikepattyn.nl")]
    [InlineData("kapsalon", "Production", "kapsalon.mikepattyn.nl")]
    [InlineData("fish", "Development", "fish-dev.mikepattyn.nl")]
    [InlineData("fish", "Production", "fish.mikepattyn.nl")]
    public void For_ReturnsSingleLevelHostname(
        string appSlug,
        string environmentName,
        string expected
    )
    {
        var environment = environmentName switch
        {
            "Development" => DeploymentEnvironment.Development,
            "Staging" => DeploymentEnvironment.Staging,
            "Production" => DeploymentEnvironment.Production,
            _ => throw new ArgumentOutOfRangeException(nameof(environmentName)),
        };

        Assert.Equal(expected, AppHostnames.For(appSlug, environment, PlatformDomain));
    }

    [Theory]
    [InlineData("kapsalon", "Development", "kapsalon-dev")]
    [InlineData("fish", "Production", "fish")]
    public void GetRecordName_ReturnsRoute53Label(
        string appSlug,
        string environmentName,
        string expected
    )
    {
        var environment = environmentName switch
        {
            "Development" => DeploymentEnvironment.Development,
            "Production" => DeploymentEnvironment.Production,
            _ => throw new ArgumentOutOfRangeException(nameof(environmentName)),
        };

        Assert.Equal(expected, AppHostnames.GetRecordName(appSlug, environment));
    }
}

public class DeploymentEnvironmentTests
{
    [Fact]
    public void EqualityComparesNameAndSubdomain()
    {
        Assert.Equal(DeploymentEnvironment.Development, new DeploymentEnvironment("Development", "dev"));
        Assert.NotEqual(DeploymentEnvironment.Development, DeploymentEnvironment.Staging);
    }
}

public class ApiRateLimitOptionsTests
{
    [Fact]
    public void ForEnvironment_ReturnsEnvironmentSpecificLimits()
    {
        var development = ApiRateLimitOptions.ForEnvironment(DeploymentEnvironment.Development);
        var staging = ApiRateLimitOptions.ForEnvironment(DeploymentEnvironment.Staging);
        var production = ApiRateLimitOptions.ForEnvironment(DeploymentEnvironment.Production);

        Assert.True(development.ThrottlingRateLimit > production.ThrottlingRateLimit);
        Assert.True(staging.ThrottlingRateLimit > production.ThrottlingRateLimit);
        Assert.Equal(300, development.WafEvaluationWindowSeconds);
    }
}
