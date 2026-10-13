namespace vCore.Domain.Tenants;

public sealed class Tenant
{
    public const int MaxTenantCodeLength = 30;
    public const int MaxTenantNameLength = 200;
    public const int MaxTimeZoneLength = 100;
    public const int MaxLocaleLength = 20;
    public const int MaxCountryCodeLength = 2;
    public const int MaxContactEmailLength = 320;
    public const int MaxContactPhoneLength = 30;

    public static class Statuses
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
        public const string Suspended = "Suspended";

        public static readonly IReadOnlyCollection<string> AllowedValues = new[] { Active, Inactive, Suspended };
    }

    public static class TenantTypes
    {
        public const string Customer = "Customer";
        public const string Internal = "Internal";
        public const string Trial = "Trial";

        public static readonly IReadOnlyCollection<string> AllowedValues = new[] { Customer, Internal, Trial };
    }

    private Tenant()
    {
    }

    public Guid TenantId { get; private set; }
    public string TenantCode { get; private set; } = string.Empty;
    public string TenantName { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public string TenantType { get; private set; } = string.Empty;
    public string TimeZone { get; private set; } = string.Empty;
    public string Locale { get; private set; } = string.Empty;
    public string CountryCode { get; private set; } = string.Empty;
    public string ContactEmail { get; private set; } = string.Empty;
    public string? ContactPhone { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public static Tenant Create(
        string tenantCode,
        string tenantName,
        string tenantType,
        string timeZone,
        string locale,
        string countryCode,
        string contactEmail,
        string? contactPhone,
        string? status,
        Guid? createdBy,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new ArgumentException("Tenant code is required.", nameof(tenantCode));
        }

        if (string.IsNullOrWhiteSpace(tenantName))
        {
            throw new ArgumentException("Tenant name is required.", nameof(tenantName));
        }

        if (string.IsNullOrWhiteSpace(tenantType))
        {
            throw new ArgumentException("Tenant type is required.", nameof(tenantType));
        }

        if (!TenantTypes.AllowedValues.Contains(tenantType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Tenant type must be one of: {string.Join(", ", TenantTypes.AllowedValues)}.", nameof(tenantType));
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            throw new InvalidOperationException("Tenant registration status is undefined and must be resolved by a configured business policy.");
        }

        if (!Statuses.AllowedValues.Contains(status, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Status must be one of: {string.Join(", ", Statuses.AllowedValues)}.", nameof(status));
        }

        var entity = new Tenant
        {
            TenantId = Guid.NewGuid(),
            TenantCode = tenantCode.Trim(),
            TenantName = tenantName.Trim(),
            TenantType = tenantType,
            TimeZone = timeZone.Trim(),
            Locale = locale.Trim(),
            CountryCode = countryCode.Trim(),
            ContactEmail = contactEmail.Trim(),
            ContactPhone = string.IsNullOrWhiteSpace(contactPhone) ? null : contactPhone.Trim(),
            Status = status,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            CreatedBy = createdBy,
            UpdatedAt = null,
            UpdatedBy = null
        };

        return entity;
    }

    public void UpdateProfile(
        string tenantName,
        string timeZone,
        string locale,
        string countryCode,
        string contactEmail,
        string? contactPhone,
        Guid? updatedBy,
        DateTime? updatedAt = null)
    {
        TenantName = tenantName.Trim();
        TimeZone = timeZone.Trim();
        Locale = locale.Trim();
        CountryCode = countryCode.Trim();
        ContactEmail = contactEmail.Trim();
        ContactPhone = string.IsNullOrWhiteSpace(contactPhone) ? null : contactPhone.Trim();
        UpdatedAt = updatedAt ?? DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void ChangeStatus(string status, Guid? updatedBy, DateTime? updatedAt = null)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Status is required.", nameof(status));
        }

        if (!Statuses.AllowedValues.Contains(status, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Status must be one of: {string.Join(", ", Statuses.AllowedValues)}.", nameof(status));
        }

        Status = status;
        UpdatedAt = updatedAt ?? DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void SetRowVersion(byte[] rowVersion)
    {
        RowVersion = rowVersion ?? throw new ArgumentNullException(nameof(rowVersion));
    }
}
