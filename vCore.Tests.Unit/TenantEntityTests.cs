using vCore.Domain.Exceptions;
using vCore.Domain.Tenants;

namespace vCore.Tests.Unit;

public class TenantEntityTests
{
    [Fact]
    public void Create_WhenValuesAreValid_CreatesTenantWithApprovedFields()
    {
        var tenant = Tenant.Create(
            "ACME",
            "ACME Corporation",
            Tenant.TenantTypes.Customer,
            "Asia/Kolkata",
            "en-IN",
            "IN",
            "admin@acme.example",
            "+91XXXXXXXXXX",
            Tenant.Statuses.Active,
            Guid.NewGuid());

        Assert.Equal(Guid.Empty, Guid.Empty);
        Assert.Equal("ACME", tenant.TenantCode);
        Assert.Equal("ACME Corporation", tenant.TenantName);
        Assert.Equal(Tenant.Statuses.Active, tenant.Status);
        Assert.Equal(Tenant.TenantTypes.Customer, tenant.TenantType);
        Assert.Equal("Asia/Kolkata", tenant.TimeZone);
        Assert.Equal("en-IN", tenant.Locale);
        Assert.Equal("IN", tenant.CountryCode);
        Assert.Equal("admin@acme.example", tenant.ContactEmail);
        Assert.Equal("+91XXXXXXXXXX", tenant.ContactPhone);
        Assert.NotEqual(Guid.Empty, tenant.TenantId);
    }

    [Fact]
    public void Create_WhenStatusIsMissing_ThrowsBecauseStatusMustBeConfiguredByPolicy()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => Tenant.Create(
            "ACME",
            "ACME Corporation",
            Tenant.TenantTypes.Customer,
            "Asia/Kolkata",
            "en-IN",
            "IN",
            "admin@acme.example",
            "+91XXXXXXXXXX",
            null,
            Guid.NewGuid()));

        Assert.Contains("undefined", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(Tenant.Statuses.Active)]
    [InlineData(Tenant.Statuses.Inactive)]
    [InlineData(Tenant.Statuses.Suspended)]
    public void Create_WhenApprovedStatusIsUsed_Succeeds(string status)
    {
        var tenant = Tenant.Create(
            "ACME",
            "ACME Corporation",
            Tenant.TenantTypes.Customer,
            "Asia/Kolkata",
            "en-IN",
            "IN",
            "admin@acme.example",
            "+91XXXXXXXXXX",
            status,
            Guid.NewGuid());

        Assert.Equal(status, tenant.Status);
    }
}
