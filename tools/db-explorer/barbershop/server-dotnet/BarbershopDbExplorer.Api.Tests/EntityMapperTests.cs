using Amazon.DynamoDBv2.Model;
using BarbershopDbExplorer.Api.Models;
using Xunit;

namespace BarbershopDbExplorer.Api.Tests;

public class EntityMapperTests
{
    private const string TenantId = "sabunandsteel";

    // --- MapToItem ---

    [Fact]
    public void MapToItem_StaffMember_WritesKeysAndFields()
    {
        var data = new StaffMemberData("marcus", TenantId, "Marcus");

        var item = EntityMapper.MapToItem(data);

        Assert.Equal("TENANT#sabunandsteel", item["PK"].S);
        Assert.Equal("STAFF#marcus", item["SK"].S);
        Assert.Equal("StaffMember", item["EntityType"].S);
        Assert.Equal("marcus", item["StaffId"].S);
        Assert.Equal("Marcus", item["Name"].S);
    }

    [Fact]
    public void MapToItem_Appointment_WritesGsiKeysAndFields()
    {
        var data = new AppointmentData(
            "appt-1",
            TenantId,
            "cust-1",
            "cut",
            "marcus",
            "2026-07-25",
            "09:00",
            "Jane Doe",
            "2026-07-01T00:00:00.000Z"
        );

        var item = EntityMapper.MapToItem(data);

        Assert.Equal("TENANT#sabunandsteel", item["PK"].S);
        Assert.Equal("APPOINTMENT#appt-1", item["SK"].S);
        Assert.Equal("CUSTOMER#cust-1", item["GSI1PK"].S);
        Assert.Equal("TENANT#sabunandsteel#APPOINTMENT#appt-1", item["GSI1SK"].S);
        Assert.Equal("cust-1", item["CustomerId"].S);
        Assert.Equal("cut", item["ServiceId"].S);
        Assert.Equal("marcus", item["StaffId"].S);
        Assert.Equal("Jane Doe", item["CustomerDisplayName"].S);
    }

    [Fact]
    public void MapToItem_Customer_UsesPlatformPartition()
    {
        var data = new CustomerData("principal-1", "a@b.com", "Jane", "Jane G", "2026-01-01", "2026-01-02");

        var item = EntityMapper.MapToItem(data);

        Assert.Equal("TENANT#PLATFORM", item["PK"].S);
        Assert.Equal("CUSTOMER#principal-1", item["SK"].S);
        Assert.Equal("Jane", item["DisplayName"].S);
    }

    [Fact]
    public void MapToItem_Service_WritesNumericFields()
    {
        var data = new ServiceData("cut", TenantId, 30, 2500);

        var item = EntityMapper.MapToItem(data);

        Assert.Equal("30", item["DurationMinutes"].N);
        Assert.Equal("2500", item["PriceCents"].N);
    }

    // --- MapFromItem (this is where the reported blank-field bug would show up if it's a mapping bug) ---

    [Fact]
    public void MapStaffMember_PopulatesName()
    {
        var item = new Dictionary<string, AttributeValue>
        {
            ["StaffId"] = new("marcus"),
            ["TenantId"] = new(TenantId),
            ["Name"] = new("Marcus"),
        };

        var staff = EntityMapper.MapStaffMember(item);

        Assert.Equal("Marcus", staff.Name);
        Assert.Equal("marcus", staff.StaffId);
    }

    [Fact]
    public void MapCustomer_PopulatesDisplayName()
    {
        var item = new Dictionary<string, AttributeValue>
        {
            ["PrincipalId"] = new("principal-1"),
            ["Email"] = new("a@b.com"),
            ["DisplayName"] = new("Jane Doe"),
            ["GoogleName"] = new("Jane G"),
            ["CreatedAt"] = new("2026-01-01"),
            ["UpdatedAt"] = new("2026-01-02"),
        };

        var customer = EntityMapper.MapCustomer(item);

        Assert.Equal("Jane Doe", customer.DisplayName);
    }

    [Fact]
    public void MapAppointment_PopulatesCustomerServiceAndStaffIds()
    {
        var item = new Dictionary<string, AttributeValue>
        {
            ["AppointmentId"] = new("appt-1"),
            ["TenantId"] = new(TenantId),
            ["CustomerId"] = new("cust-1"),
            ["ServiceId"] = new("cut"),
            ["StaffId"] = new("marcus"),
            ["Date"] = new("2026-07-25"),
            ["TimeSlot"] = new("09:00"),
            ["CustomerDisplayName"] = new("Jane Doe"),
            ["CreatedAt"] = new("2026-07-01T00:00:00.000Z"),
        };

        var appointment = EntityMapper.MapAppointment(item);

        Assert.Equal("cust-1", appointment.CustomerId);
        Assert.Equal("cut", appointment.ServiceId);
        Assert.Equal("marcus", appointment.StaffId);
    }

    [Fact]
    public void MapCustomer_DefaultsGoogleNameWhenMissing()
    {
        var item = new Dictionary<string, AttributeValue>
        {
            ["PrincipalId"] = new("principal-1"),
            ["Email"] = new("a@b.com"),
            ["DisplayName"] = new("Jane Doe"),
            ["CreatedAt"] = new("2026-01-01"),
            ["UpdatedAt"] = new("2026-01-02"),
        };

        var customer = EntityMapper.MapCustomer(item);

        Assert.Equal("", customer.GoogleName);
    }

    // --- SlotLock + orphans ---

    [Fact]
    public void MapSlotLockFromAppointment_BuildsSlotKey()
    {
        var appointment = new AppointmentData(
            "appt-1",
            TenantId,
            "cust-1",
            "cut",
            "marcus",
            "2026-07-25",
            "09:00",
            "Jane Doe",
            "2026-07-01T00:00:00.000Z"
        );

        var slotLock = EntityMapper.MapSlotLockFromAppointment(appointment);

        Assert.Equal("TENANT#sabunandsteel", slotLock["PK"].S);
        Assert.Equal("SLOT#2026-07-25#09:00#marcus", slotLock["SK"].S);
        Assert.Equal("SlotLock", slotLock["EntityType"].S);
        Assert.Equal("appt-1", slotLock["AppointmentId"].S);
    }

    [Fact]
    public void MapSlotLockOrphan_MapsAllFields()
    {
        var item = new Dictionary<string, AttributeValue>
        {
            ["PK"] = new("TENANT#sabunandsteel"),
            ["SK"] = new("SLOT#2026-07-25#09:00#marcus"),
            ["TenantId"] = new(TenantId),
            ["StaffId"] = new("marcus"),
            ["Date"] = new("2026-07-25"),
            ["TimeSlot"] = new("09:00"),
            ["AppointmentId"] = new("appt-1"),
        };

        var orphan = EntityMapper.MapSlotLockOrphan(item);

        Assert.Equal("appt-1", orphan.AppointmentId);
        Assert.Equal("marcus", orphan.StaffId);
    }

    [Fact]
    public void AppointmentExistsKey_BuildsPkAndSk()
    {
        var (pk, sk) = EntityMapper.AppointmentExistsKey(TenantId, "appt-1");

        Assert.Equal("TENANT#sabunandsteel", pk);
        Assert.Equal("APPOINTMENT#appt-1", sk);
    }

    // --- ListQuery ---

    [Fact]
    public void ListQuery_Customer_UsesPlatformPkAndCustomerPrefix()
    {
        var (pk, skPrefix, exactSk) = EntityMapper.ListQuery("Customer", TenantId);

        Assert.Equal("TENANT#PLATFORM", pk);
        Assert.Equal("CUSTOMER#", skPrefix);
        Assert.False(exactSk);
    }

    [Fact]
    public void ListQuery_TenantProfile_UsesExactSk()
    {
        var (pk, skPrefix, exactSk) = EntityMapper.ListQuery("TenantProfile", TenantId);

        Assert.Equal("TENANT#sabunandsteel", pk);
        Assert.Equal("PROFILE", skPrefix);
        Assert.True(exactSk);
    }

    // --- ToListItem / labels ---

    [Fact]
    public void ToListItem_StaffMember_IncludesNameInLabel()
    {
        var item = new Dictionary<string, AttributeValue>
        {
            ["PK"] = new("TENANT#sabunandsteel"),
            ["SK"] = new("STAFF#marcus"),
            ["StaffId"] = new("marcus"),
            ["TenantId"] = new(TenantId),
            ["Name"] = new("Marcus"),
        };

        var listItem = EntityMapper.ToListItem("StaffMember", item);

        Assert.Equal("Marcus (marcus)", listItem.Label);
        var data = Assert.IsType<StaffMemberData>(listItem.Data);
        Assert.Equal("Marcus", data.Name);
    }

    [Fact]
    public void ItemLabel_StaffMember_FallsBackToStaffWhenNameBlank()
    {
        var data = new StaffMemberData("marcus", TenantId, "");
        Assert.Equal("Staff (marcus)", EntityMapper.ItemLabel("StaffMember", data));
    }

    [Fact]
    public void ItemLabel_Customer_UsesDisplayNameWhenPresent()
    {
        var data = new CustomerData("principal-1", "a@b.com", "Jane Doe", "", "2026-01-01", "2026-01-02");
        Assert.Equal("Jane Doe (principal-1)", EntityMapper.ItemLabel("Customer", data));
    }

    [Fact]
    public void ItemLabel_Appointment_FallsBackToAppointmentIdWhenDisplayNameBlank()
    {
        var data = new AppointmentData(
            "appt-1",
            TenantId,
            "cust-1",
            "cut",
            "marcus",
            "2026-07-25",
            "09:00",
            "",
            "2026-07-01T00:00:00.000Z"
        );
        Assert.Equal("2026-07-25 09:00 — appt-1", EntityMapper.ItemLabel("Appointment", data));
    }
}
