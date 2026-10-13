using System.ComponentModel.DataAnnotations;
using vCore.Application.DTOs;
using vCore.Domain.Exceptions;
using vCore.Domain.Tenants;

namespace vCore.Application.Validation;

public static class TenantRequestValidator
{
    public static void ValidateCreate(CreateTenantRequest request)
    {
        if (request is null)
        {
            throw new TenantValidationException("Tenant creation request is required.");
        }

        ValidateTenantCode(request.TenantCode);
        ValidateTenantName(request.TenantName);
        ValidateTenantType(request.TenantType);
        ValidateTimeZone(request.TimeZone);
        ValidateLocale(request.Locale);
        ValidateCountryCode(request.CountryCode);
        ValidateContactEmail(request.ContactEmail);
        ValidateContactPhone(request.ContactPhone);
    }

    public static void ValidateUpdate(Guid tenantId, UpdateTenantRequest request)
    {
        if (tenantId == Guid.Empty)
        {
            throw new TenantValidationException("Tenant identifier is required.");
        }

        if (request is null)
        {
            throw new TenantValidationException("Tenant update request is required.");
        }

        ValidateTenantName(request.TenantName);
        ValidateTimeZone(request.TimeZone);
        ValidateLocale(request.Locale);
        ValidateCountryCode(request.CountryCode);
        ValidateContactEmail(request.ContactEmail);
        ValidateContactPhone(request.ContactPhone);
        ValidateRowVersion(request.RowVersion, "Tenant update request is invalid.");
    }

    public static void ValidateStatusChange(Guid tenantId, ChangeTenantStatusRequest request)
    {
        if (tenantId == Guid.Empty)
        {
            throw new TenantValidationException("Tenant identifier is required.");
        }

        if (request is null)
        {
            throw new TenantValidationException("Tenant status change request is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            throw new TenantValidationException("Status is required.");
        }

        if (!Tenant.Statuses.AllowedValues.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
        {
            throw new TenantValidationException($"Status must be one of: {string.Join(", ", Tenant.Statuses.AllowedValues)}.");
        }

        ValidateRowVersion(request.RowVersion, "Tenant status change request is invalid.");
    }

    private static void ValidateTenantCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Tenant code is required.");
        }

        if (value.Length > Tenant.MaxTenantCodeLength)
        {
            throw new TenantValidationException($"Tenant code cannot exceed {Tenant.MaxTenantCodeLength} characters.");
        }
    }

    private static void ValidateTenantName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Tenant name is required.");
        }

        if (value.Length > Tenant.MaxTenantNameLength)
        {
            throw new TenantValidationException($"Tenant name cannot exceed {Tenant.MaxTenantNameLength} characters.");
        }
    }

    private static void ValidateTenantType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Tenant type is required.");
        }

        if (!Tenant.TenantTypes.AllowedValues.Contains(value, StringComparer.OrdinalIgnoreCase))
        {
            throw new TenantValidationException($"Tenant type must be one of: {string.Join(", ", Tenant.TenantTypes.AllowedValues)}.");
        }
    }

    private static void ValidateTimeZone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Time zone is required.");
        }

        if (value.Length > Tenant.MaxTimeZoneLength)
        {
            throw new TenantValidationException($"Time zone cannot exceed {Tenant.MaxTimeZoneLength} characters.");
        }
    }

    private static void ValidateLocale(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Locale is required.");
        }

        if (value.Length > Tenant.MaxLocaleLength)
        {
            throw new TenantValidationException($"Locale cannot exceed {Tenant.MaxLocaleLength} characters.");
        }
    }

    private static void ValidateCountryCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Country code is required.");
        }

        if (value.Length != Tenant.MaxCountryCodeLength)
        {
            throw new TenantValidationException("Country code must be exactly 2 characters.");
        }
    }

    private static void ValidateContactEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException("Contact email is required.");
        }

        if (value.Length > Tenant.MaxContactEmailLength)
        {
            throw new TenantValidationException($"Contact email cannot exceed {Tenant.MaxContactEmailLength} characters.");
        }

        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(value))
        {
            throw new TenantValidationException("Contact email must be a valid email address.");
        }
    }

    private static void ValidateContactPhone(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (value.Length > Tenant.MaxContactPhoneLength)
        {
            throw new TenantValidationException($"Contact phone cannot exceed {Tenant.MaxContactPhoneLength} characters.");
        }
    }

    private static void ValidateRowVersion(string value, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new TenantValidationException(errorMessage);
        }
    }
}
