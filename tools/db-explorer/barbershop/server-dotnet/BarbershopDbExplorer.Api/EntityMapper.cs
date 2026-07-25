using Amazon.DynamoDBv2.Model;
using BarbershopDbExplorer.Api.Models;

namespace BarbershopDbExplorer.Api;

public static class EntityMapper
{
    // --- MapToItem ---

    public static Dictionary<string, AttributeValue> MapToItem(TenantProfileData data) =>
        new()
        {
            ["PK"] = new(DynamoKeys.TenantPk(data.TenantId)),
            ["SK"] = new(DynamoKeys.ProfileSk()),
            ["EntityType"] = new("TenantProfile"),
            ["TenantId"] = new(data.TenantId),
            ["DisplayName"] = new(data.DisplayName),
            ["Address"] = new(data.Address),
            ["Hours"] = new(data.Hours),
            ["Email"] = new(data.Email),
            ["Phone"] = new(data.Phone),
            ["HeroImageSrc"] = new(data.HeroImageSrc),
            ["Timezone"] = new(string.IsNullOrWhiteSpace(data.Timezone) ? DynamoKeys.DefaultTimezone : data.Timezone),
        };

    public static Dictionary<string, AttributeValue> MapToItem(StaffMemberData data) =>
        new()
        {
            ["PK"] = new(DynamoKeys.TenantPk(data.TenantId)),
            ["SK"] = new(DynamoKeys.StaffSk(data.StaffId)),
            ["EntityType"] = new("StaffMember"),
            ["StaffId"] = new(data.StaffId),
            ["TenantId"] = new(data.TenantId),
            ["Name"] = new(data.Name),
        };

    public static Dictionary<string, AttributeValue> MapToItem(ServiceData data) =>
        new()
        {
            ["PK"] = new(DynamoKeys.TenantPk(data.TenantId)),
            ["SK"] = new(DynamoKeys.ServiceSk(data.ServiceId)),
            ["EntityType"] = new("Service"),
            ["ServiceId"] = new(data.ServiceId),
            ["TenantId"] = new(data.TenantId),
            ["DurationMinutes"] = new AttributeValue { N = data.DurationMinutes.ToString() },
            ["PriceCents"] = new AttributeValue { N = data.PriceCents.ToString() },
        };

    public static Dictionary<string, AttributeValue> MapToItem(AppointmentData data) =>
        new()
        {
            ["PK"] = new(DynamoKeys.TenantPk(data.TenantId)),
            ["SK"] = new(DynamoKeys.AppointmentSk(data.AppointmentId)),
            ["GSI1PK"] = new(DynamoKeys.CustomerGsi1Pk(data.CustomerId)),
            ["GSI1SK"] = new(DynamoKeys.AppointmentGsi1Sk(data.TenantId, data.AppointmentId)),
            ["EntityType"] = new("Appointment"),
            ["AppointmentId"] = new(data.AppointmentId),
            ["TenantId"] = new(data.TenantId),
            ["CustomerId"] = new(data.CustomerId),
            ["ServiceId"] = new(data.ServiceId),
            ["StaffId"] = new(data.StaffId),
            ["Date"] = new(data.Date),
            ["TimeSlot"] = new(data.TimeSlot),
            ["CustomerDisplayName"] = new(data.CustomerDisplayName),
            ["CreatedAt"] = new(data.CreatedAt),
        };

    public static Dictionary<string, AttributeValue> MapToItem(CustomerData data) =>
        new()
        {
            ["PK"] = new(DynamoKeys.TenantPk(DynamoKeys.PlatformTenant)),
            ["SK"] = new(DynamoKeys.CustomerSk(data.PrincipalId)),
            ["EntityType"] = new("Customer"),
            ["PrincipalId"] = new(data.PrincipalId),
            ["Email"] = new(data.Email),
            ["DisplayName"] = new(data.DisplayName),
            ["GoogleName"] = new(data.GoogleName ?? string.Empty),
            ["CreatedAt"] = new(data.CreatedAt),
            ["UpdatedAt"] = new(data.UpdatedAt),
        };

    // --- MapFromItem ---

    private static string GetString(Dictionary<string, AttributeValue> item, string key) =>
        item.TryGetValue(key, out var value) ? value.S ?? string.Empty : string.Empty;

    public static TenantProfileData MapTenantProfile(Dictionary<string, AttributeValue> item)
    {
        var timezone = GetString(item, "Timezone");
        return new TenantProfileData(
            GetString(item, "TenantId"),
            GetString(item, "DisplayName"),
            GetString(item, "Address"),
            GetString(item, "Hours"),
            GetString(item, "Email"),
            GetString(item, "Phone"),
            GetString(item, "HeroImageSrc"),
            string.IsNullOrWhiteSpace(timezone) ? DynamoKeys.DefaultTimezone : timezone
        );
    }

    public static StaffMemberData MapStaffMember(Dictionary<string, AttributeValue> item) =>
        new(GetString(item, "StaffId"), GetString(item, "TenantId"), GetString(item, "Name"));

    public static ServiceData MapService(Dictionary<string, AttributeValue> item) =>
        new(
            GetString(item, "ServiceId"),
            GetString(item, "TenantId"),
            int.Parse(item["DurationMinutes"].N),
            int.Parse(item["PriceCents"].N)
        );

    public static AppointmentData MapAppointment(Dictionary<string, AttributeValue> item) =>
        new(
            GetString(item, "AppointmentId"),
            GetString(item, "TenantId"),
            GetString(item, "CustomerId"),
            GetString(item, "ServiceId"),
            GetString(item, "StaffId"),
            GetString(item, "Date"),
            GetString(item, "TimeSlot"),
            GetString(item, "CustomerDisplayName"),
            GetString(item, "CreatedAt")
        );

    public static CustomerData MapCustomer(Dictionary<string, AttributeValue> item) =>
        new(
            GetString(item, "PrincipalId"),
            GetString(item, "Email"),
            GetString(item, "DisplayName"),
            GetString(item, "GoogleName"),
            GetString(item, "CreatedAt"),
            GetString(item, "UpdatedAt")
        );

    public static object MapFromItem(string entityType, Dictionary<string, AttributeValue> item) =>
        entityType switch
        {
            "TenantProfile" => MapTenantProfile(item),
            "StaffMember" => MapStaffMember(item),
            "Service" => MapService(item),
            "Appointment" => MapAppointment(item),
            "Customer" => MapCustomer(item),
            _ => throw new ArgumentException($"Unknown entity type: {entityType}", nameof(entityType)),
        };

    // --- SlotLock ---

    public static Dictionary<string, AttributeValue> MapSlotLockFromAppointment(AppointmentData appointment) =>
        new()
        {
            ["PK"] = new(DynamoKeys.TenantPk(appointment.TenantId)),
            ["SK"] = new(DynamoKeys.SlotLockSk(appointment.Date, appointment.TimeSlot, appointment.StaffId)),
            ["EntityType"] = new("SlotLock"),
            ["TenantId"] = new(appointment.TenantId),
            ["StaffId"] = new(appointment.StaffId),
            ["Date"] = new(appointment.Date),
            ["TimeSlot"] = new(appointment.TimeSlot),
            ["AppointmentId"] = new(appointment.AppointmentId),
        };

    public static SlotLockOrphan MapSlotLockOrphan(Dictionary<string, AttributeValue> item) =>
        new(
            GetString(item, "PK"),
            GetString(item, "SK"),
            GetString(item, "TenantId"),
            GetString(item, "StaffId"),
            GetString(item, "Date"),
            GetString(item, "TimeSlot"),
            GetString(item, "AppointmentId")
        );

    public static (string Pk, string Sk) AppointmentExistsKey(string tenantId, string appointmentId) =>
        (DynamoKeys.TenantPk(tenantId), DynamoKeys.AppointmentSk(appointmentId));

    // --- List query / list item / labels ---

    public static (string Pk, string SkPrefix, bool ExactSk) ListQuery(string entityType, string tenantId) =>
        (DynamoKeys.QueryPk(entityType, tenantId), DynamoKeys.SkPrefix(entityType), entityType == "TenantProfile");

    public static ListItem ToListItem(string entityType, Dictionary<string, AttributeValue> item)
    {
        var data = MapFromItem(entityType, item);
        return new ListItem
        {
            PK = GetString(item, "PK"),
            SK = GetString(item, "SK"),
            EntityType = entityType,
            Label = ItemLabel(entityType, data),
            Data = data,
        };
    }

    public static string ItemLabel(string entityType, object data) =>
        entityType switch
        {
            "TenantProfile" => LabelOrFallback(
                ((TenantProfileData)data).DisplayName,
                ((TenantProfileData)data).TenantId
            ),
            "StaffMember" => $"{LabelOrFallback(((StaffMemberData)data).Name, "Staff")} ({((StaffMemberData)data).StaffId})",
            "Service" => $"{((ServiceData)data).ServiceId} — {((ServiceData)data).DurationMinutes}m / {((ServiceData)data).PriceCents}c",
            "Appointment" => AppointmentLabel((AppointmentData)data),
            "Customer" => $"{LabelOrFallback(((CustomerData)data).DisplayName, "Customer")} ({((CustomerData)data).PrincipalId})",
            _ => throw new ArgumentException($"Unknown entity type: {entityType}", nameof(entityType)),
        };

    private static string AppointmentLabel(AppointmentData data) =>
        $"{data.Date} {data.TimeSlot} — {LabelOrFallback(data.CustomerDisplayName, data.AppointmentId)}";

    private static string LabelOrFallback(string value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value;
}
