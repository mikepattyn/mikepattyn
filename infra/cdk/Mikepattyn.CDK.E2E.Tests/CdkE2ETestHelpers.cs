namespace Mikepattyn.CDK.E2E.Tests;

internal static class CdkE2ETestHelpers
{
    internal static IDisposable UseCdkWorkingDirectory()
    {
        var previous = Directory.GetCurrentDirectory();
        var repoRoot = FindRepoRoot();
        Directory.SetCurrentDirectory(Path.Combine(repoRoot, "infra", "cdk"));
        return new RestoreWorkingDirectory(previous);
    }

    private sealed class RestoreWorkingDirectory(string previous) : IDisposable
    {
        public void Dispose() => Directory.SetCurrentDirectory(previous);
    }

    private static string FindRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "Makefile")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }
}
