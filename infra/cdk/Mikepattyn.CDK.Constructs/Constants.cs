namespace Mikepattyn.CDK.Constructs;

// Copy Constants.Deployment.cs.example to Constants.Deployment.cs before cdk synth or deploy.
public static partial class Constants
{
    public const string PlatformName = "Mikepattyn";

    public static class Domains
    {
        public const string Mikepattyn = "mikepattyn.nl";
        public const string AlienButNice = "alienbutnice.nl";
    }

    public static class Apps
    {
        public const string Kapsalon = "Kapsalon";
        public const string Fish = "Fish";
        public const string Mikepattyn = "Mikepattyn";
        public const string AlienButNice = "AlienButNice";

        public const string KapsalonSlug = "barbershop";
        public const string FishSlug = "gofish";
        public const string MikepattynSlug = "mikepattyn";
        public const string AlienButNiceSlug = "alienbutnice";
    }

    public static class Stacks
    {
        private static readonly string PlatformRoot = PlatformName;

        public static string GetPlatformStack(string stackName) => $"{PlatformRoot}-{stackName}-Stack";

        public static string GetAppStack(string appName, string stackName) =>
            $"{appName}-{stackName}-Stack";

        public static string GetAppStackName(string appName, string stackName) =>
            GetAppStack(appName, stackName);

        public static readonly string Domain = GetPlatformStack("Domain");
        public static readonly string AlienButNiceDomain = GetAppStack(Apps.AlienButNice, "Domain");
        public static readonly string Auth = GetPlatformStack("Auth");

        public static readonly string KapsalonFrontend = GetAppStack(Apps.Kapsalon, "Frontend");
        public static readonly string KapsalonBackend = GetAppStack(Apps.Kapsalon, "Backend");
        public static readonly string FishBackend = GetAppStack(Apps.Fish, "Backend");
        public static readonly string FishFrontend = GetAppStack(Apps.Fish, "Frontend");
        public static readonly string MikepattynBrandFrontend = GetAppStack(Apps.Mikepattyn, "BrandFrontend");
        public static readonly string AlienButNiceBrandFrontend = GetAppStack(Apps.AlienButNice, "BrandFrontend");
    }
}
