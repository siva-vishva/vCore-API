using vCore.Application.DTOs;
using vCore.Application.Validation;
using vCore.Domain.Exceptions;
using vCore.Domain.Tenants;

namespace vCore.Tests.Unit;

public class TenantRequestValidatorTests
{
    [Fact]
    public void CreateValidator_WhenTenantCodeMissing_ThrowsValidationException()
    {
        var request = new CreateTenantRequest
        {
            TenantCode = string.Empty,
            TenantName = "ACME Corporation",
            TenantType = Tenant.TenantTypes.Customer,
            TimeZone = "Asia/Kolkata",
            Locale = "en-IN",
            CountryCode = "IN",
            ContactEmail = "admin@acme.example"
        };

        var ex = Assert.Throws<TenantValidationException>(() => TenantRequestValidator.ValidateCreate(request));
        Assert.Contains("Tenant code", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateValidator_WhenEmailIsInvalid_ThrowsValidationException()
    {
        var request = new CreateTenantRequest
        {
            TenantCode = "ACME",
            TenantName = "ACME Corporation",
            TenantType = Tenant.TenantTypes.Customer,
            TimeZone = "Asia/Kolkata",
            Locale = "en-IN",
            CountryCode = "IN",
            ContactEmail = "not-an-email"
        };

        var ex = Assert.Throws<TenantValidationException>(() => TenantRequestValidator.ValidateCreate(request));
        Assert.Contains("email", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("Inactive")]
    [InlineData("Suspended")]
    public void StatusValidator_WhenApprovedValueIsUsed_Succeeds(string status)
    {
        var request = new ChangeTenantStatusRequest
        {
            Status = status,
            RowVersion = Convert.ToBase64String(new byte[] { 1, 2, 3 })
        };

        var exception = Record.Exception(() => TenantRequestValidator.ValidateStatusChange(Guid.NewGuid(), request));
        Assert.Null(exception);
    }
}
