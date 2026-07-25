namespace BarbershopDbExplorer.Api;

public static class DynamoKeys
{
    public const string AppName = "Kapsalon";
    public const string PlatformTenant = "PLATFORM";
    public const string DefaultTimezone = "Europe/Amsterdam";

    private static readonly Dictionary<string, string> UiEnvToDeployment = new()
    {
        ["dev"] = "Development",
        ["acc"] = "Staging",
        ["prod"] = "Production",
    };

    public static string TenantPk(string tenantId) => $"TENANT#{tenantId}";

    public static string CustomerSk(string principalId) => $"CUSTOMER#{principalId}";

    public static string AppointmentSk(string appointmentId) => $"APPOINTMENT#{appointmentId}";

    public static string CustomerGsi1Pk(string principalId) => $"CUSTOMER#{principalId}";

    public static string AppointmentGsi1Sk(string tenantId, string appointmentId) =>
        $"TENANT#{tenantId}#APPOINTMENT#{appointmentId}";

    public static string ServiceSk(string serviceId) => $"SERVICE#{serviceId}";

    public static string ProfileSk() => "PROFILE";

    public static string StaffSk(string staffId) => $"STAFF#{staffId}";

    public static string SlotLockSk(string date, string timeSlot, string staffId) =>
        $"SLOT#{date}#{timeSlot}#{staffId}";

    public static string SsmTableParameter(string deploymentEnv) =>
        $"/{AppName}/{deploymentEnv}/Application/DynamoDbTableName";

    public static string DeploymentEnv(string uiEnv)
    {
        if (!UiEnvToDeployment.TryGetValue(uiEnv, out var deployment))
        {
            throw new ArgumentException($"Invalid env. Use dev, acc, or prod.", nameof(uiEnv));
        }

        return deployment;
    }

    public static string SkPrefix(string entityType) =>
        entityType switch
        {
            "TenantProfile" => ProfileSk(),
            "StaffMember" => "STAFF#",
            "Service" => "SERVICE#",
            "Appointment" => "APPOINTMENT#",
            "Customer" => "CUSTOMER#",
            _ => throw new ArgumentException($"Unknown entity type: {entityType}", nameof(entityType)),
        };

    public static string QueryPk(string entityType, string tenantId) =>
        entityType == "Customer" ? TenantPk(PlatformTenant) : TenantPk(tenantId);
}
