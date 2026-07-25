namespace BarbershopDbExplorer.Api.Models;

public sealed record TenantProfileData(
    string TenantId,
    string DisplayName,
    string Address,
    string Hours,
    string Email,
    string Phone,
    string HeroImageSrc,
    string Timezone
);

public sealed record StaffMemberData(string StaffId, string TenantId, string Name);

public sealed record ServiceData(string ServiceId, string TenantId, int DurationMinutes, int PriceCents);

public sealed record AppointmentData(
    string AppointmentId,
    string TenantId,
    string CustomerId,
    string ServiceId,
    string StaffId,
    string Date,
    string TimeSlot,
    string CustomerDisplayName,
    string CreatedAt
);

public sealed record CustomerData(
    string PrincipalId,
    string Email,
    string DisplayName,
    string GoogleName,
    string CreatedAt,
    string UpdatedAt
);

public sealed record SlotLockOrphan(
    string PK,
    string SK,
    string TenantId,
    string StaffId,
    string Date,
    string TimeSlot,
    string AppointmentId
);
