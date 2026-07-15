namespace Mikepattyn.CDK.Constructs;

/// <summary>
/// Maps application slugs and deployment environments to single-level hostnames under the platform domain.
/// </summary>
public static class AppHostnames
{
    public static string For(string appSlug, DeploymentEnvironment environment, string platformDomain) =>
        $"{GetRecordName(appSlug, environment)}.{platformDomain}";

    public static string GetRecordName(string appSlug, DeploymentEnvironment environment)
    {
        if (!environment.IsValidDeploymentEnvironment)
        {
            throw new InvalidOperationException(
                "Record names are only valid for Development, Staging, or Production."
            );
        }

        return environment switch
        {
            var e when e == DeploymentEnvironment.Development => $"{appSlug}-dev",
            var e when e == DeploymentEnvironment.Staging => $"{appSlug}-acc",
            var e when e == DeploymentEnvironment.Production => appSlug,
            _ => throw new InvalidOperationException($"Unsupported environment: {environment.Name}"),
        };
    }

    public static string[] GetDomainNames(
        string appSlug,
        DeploymentEnvironment environment,
        string platformDomain
    ) => [For(appSlug, environment, platformDomain)];
}
