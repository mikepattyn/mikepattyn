using Xunit;

namespace BarbershopDbExplorer.Api.Tests;

public class DynamoKeysTests
{
    [Fact]
    public void TenantPk_PrefixesTenantId()
    {
        Assert.Equal("TENANT#sabunandsteel", DynamoKeys.TenantPk("sabunandsteel"));
    }

    [Fact]
    public void CustomerSk_PrefixesPrincipalId()
    {
        Assert.Equal("CUSTOMER#abc123", DynamoKeys.CustomerSk("abc123"));
    }

    [Fact]
    public void AppointmentSk_PrefixesAppointmentId()
    {
        Assert.Equal("APPOINTMENT#appt-1", DynamoKeys.AppointmentSk("appt-1"));
    }

    [Fact]
    public void CustomerGsi1Pk_MatchesCustomerSkPattern()
    {
        Assert.Equal("CUSTOMER#abc123", DynamoKeys.CustomerGsi1Pk("abc123"));
    }

    [Fact]
    public void AppointmentGsi1Sk_CombinesTenantAndAppointment()
    {
        Assert.Equal(
            "TENANT#sabunandsteel#APPOINTMENT#appt-1",
            DynamoKeys.AppointmentGsi1Sk("sabunandsteel", "appt-1")
        );
    }

    [Fact]
    public void ServiceSk_PrefixesServiceId()
    {
        Assert.Equal("SERVICE#cut", DynamoKeys.ServiceSk("cut"));
    }

    [Fact]
    public void ProfileSk_IsConstant()
    {
        Assert.Equal("PROFILE", DynamoKeys.ProfileSk());
    }

    [Fact]
    public void StaffSk_PrefixesStaffId()
    {
        Assert.Equal("STAFF#marcus", DynamoKeys.StaffSk("marcus"));
    }

    [Fact]
    public void SlotLockSk_CombinesDateTimeSlotAndStaff()
    {
        Assert.Equal("SLOT#2026-07-25#09:00#marcus", DynamoKeys.SlotLockSk("2026-07-25", "09:00", "marcus"));
    }

    [Fact]
    public void SsmTableParameter_BuildsPathForDeploymentEnv()
    {
        Assert.Equal(
            "/Kapsalon/Development/Application/DynamoDbTableName",
            DynamoKeys.SsmTableParameter("Development")
        );
    }

    [Theory]
    [InlineData("dev", "Development")]
    [InlineData("acc", "Staging")]
    [InlineData("prod", "Production")]
    public void DeploymentEnv_MapsUiEnvToDeploymentName(string uiEnv, string expected)
    {
        Assert.Equal(expected, DynamoKeys.DeploymentEnv(uiEnv));
    }

    [Fact]
    public void DeploymentEnv_ThrowsForUnknownUiEnv()
    {
        Assert.Throws<ArgumentException>(() => DynamoKeys.DeploymentEnv("staging"));
    }

    [Theory]
    [InlineData("TenantProfile", "PROFILE")]
    [InlineData("StaffMember", "STAFF#")]
    [InlineData("Service", "SERVICE#")]
    [InlineData("Appointment", "APPOINTMENT#")]
    [InlineData("Customer", "CUSTOMER#")]
    public void SkPrefix_ReturnsPrefixPerEntityType(string entityType, string expected)
    {
        Assert.Equal(expected, DynamoKeys.SkPrefix(entityType));
    }

    [Fact]
    public void SkPrefix_ThrowsForUnknownEntityType()
    {
        Assert.Throws<ArgumentException>(() => DynamoKeys.SkPrefix("Unknown"));
    }

    [Fact]
    public void QueryPk_UsesPlatformPartitionForCustomer()
    {
        Assert.Equal("TENANT#PLATFORM", DynamoKeys.QueryPk("Customer", "sabunandsteel"));
    }

    [Theory]
    [InlineData("TenantProfile")]
    [InlineData("StaffMember")]
    [InlineData("Service")]
    [InlineData("Appointment")]
    public void QueryPk_UsesTenantPartitionForNonCustomerEntities(string entityType)
    {
        Assert.Equal("TENANT#sabunandsteel", DynamoKeys.QueryPk(entityType, "sabunandsteel"));
    }
}
