namespace vCore.Application.DTOs;

public sealed class CreateTenantRequest
{
    public string TenantCode { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string TenantType { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
}

public sealed class UpdateTenantRequest
{
    public string TenantName { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class ChangeTenantStatusRequest
{
    public string Status { get; set; } = string.Empty;
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class TenantListRequest
{
    public string? Status { get; set; }
    public string? TenantType { get; set; }
    public string? TenantCode { get; set; }
    public string? TenantName { get; set; }
    public string? CountryCode { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? Sort { get; set; }
}

public sealed class TenantResponse
{
    public Guid TenantId { get; set; }
    public string TenantCode { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string TenantType { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

public sealed class ProvisionTenantResponse
{
    public Guid TenantId { get; set; }
    public string Status { get; set; } = "Accepted";
    public string Message { get; set; } = "Provisioning workflow initiated.";
}
