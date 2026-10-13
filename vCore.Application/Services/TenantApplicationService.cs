using vCore.Application.DTOs;
using vCore.Application.Validation;
using vCore.Domain.Exceptions;
using vCore.Domain.Tenants;

namespace vCore.Application.Services;

public sealed class TenantApplicationService : ITenantApplicationService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantRegistrationStatusPolicy _registrationStatusPolicy;
    private readonly IAuthenticatedTenantUserContext _authenticatedTenantUserContext;

    public TenantApplicationService(
        ITenantRepository tenantRepository,
        ITenantRegistrationStatusPolicy registrationStatusPolicy,
        IAuthenticatedTenantUserContext authenticatedTenantUserContext)
    {
        _tenantRepository = tenantRepository;
        _registrationStatusPolicy = registrationStatusPolicy;
        _authenticatedTenantUserContext = authenticatedTenantUserContext;
    }

    public async Task<TenantResponse> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        TenantRequestValidator.ValidateCreate(request);

        var existing = await _tenantRepository.GetByCodeAsync(request.TenantCode, cancellationToken);
        if (existing is not null)
        {
            throw new TenantCodeAlreadyExistsException(request.TenantCode);
        }

        var status = _registrationStatusPolicy.ResolveInitialStatus();
        var createdBy = _authenticatedTenantUserContext.GetCurrentUserId();

        var tenant = Tenant.Create(
            request.TenantCode,
            request.TenantName,
            request.TenantType,
            request.TimeZone,
            request.Locale,
            request.CountryCode,
            request.ContactEmail,
            request.ContactPhone,
            status,
            createdBy);

        await _tenantRepository.AddAsync(tenant, cancellationToken);
        return MapToResponse(tenant);
    }

    public async Task<TenantResponse?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        if (tenantId == Guid.Empty)
        {
            throw new TenantValidationException("Tenant identifier is required.");
        }

        var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken);
        return tenant is null ? null : MapToResponse(tenant);
    }

    public async Task<PagedResult<TenantResponse>> ListAsync(TenantListRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new TenantValidationException("Tenant search request is required.");
        }

        var result = await _tenantRepository.ListAsync(request, cancellationToken);
        return new PagedResult<TenantResponse>
        {
            Items = result.Items.Select(MapToResponse).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<TenantResponse> UpdateAsync(Guid tenantId, UpdateTenantRequest request, CancellationToken cancellationToken = default)
    {
        TenantRequestValidator.ValidateUpdate(tenantId, request);

        var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken)
            ?? throw new TenantNotFoundException(tenantId);

        if (!TryParseRowVersion(request.RowVersion, out var rowVersionBytes))
        {
            throw new TenantConcurrencyException("The supplied row version is invalid or missing.");
        }

        if (!tenant.RowVersion.SequenceEqual(rowVersionBytes))
        {
            throw new TenantConcurrencyException("The tenant record was modified by another process. Refresh and retry.");
        }

        tenant.UpdateProfile(
            request.TenantName,
            request.TimeZone,
            request.Locale,
            request.CountryCode,
            request.ContactEmail,
            request.ContactPhone,
            _authenticatedTenantUserContext.GetCurrentUserId(),
            DateTime.UtcNow);

        await _tenantRepository.UpdateAsync(tenant, cancellationToken);
        return MapToResponse(tenant);
    }

    public async Task<TenantResponse> ChangeStatusAsync(Guid tenantId, ChangeTenantStatusRequest request, CancellationToken cancellationToken = default)
    {
        TenantRequestValidator.ValidateStatusChange(tenantId, request);

        var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken)
            ?? throw new TenantNotFoundException(tenantId);

        if (!TryParseRowVersion(request.RowVersion, out var rowVersionBytes))
        {
            throw new TenantConcurrencyException("The supplied row version is invalid or missing.");
        }

        if (!tenant.RowVersion.SequenceEqual(rowVersionBytes))
        {
            throw new TenantConcurrencyException("The tenant record was modified by another process. Refresh and retry.");
        }

        tenant.ChangeStatus(request.Status, _authenticatedTenantUserContext.GetCurrentUserId(), DateTime.UtcNow);
        await _tenantRepository.UpdateAsync(tenant, cancellationToken);
        return MapToResponse(tenant);
    }

    public async Task<ProvisionTenantResponse> ProvisionAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken)
            ?? throw new TenantNotFoundException(tenantId);

        return new ProvisionTenantResponse
        {
            TenantId = tenant.TenantId,
            Status = "Accepted",
            Message = "Provisioning workflow initiated. The implementation is extensible for the approved platform provisioning workflow."
        };
    }

    private static TenantResponse MapToResponse(Tenant tenant)
    {
        return new TenantResponse
        {
            TenantId = tenant.TenantId,
            TenantCode = tenant.TenantCode,
            TenantName = tenant.TenantName,
            Status = tenant.Status,
            TenantType = tenant.TenantType,
            TimeZone = tenant.TimeZone,
            Locale = tenant.Locale,
            CountryCode = tenant.CountryCode,
            ContactEmail = tenant.ContactEmail,
            ContactPhone = tenant.ContactPhone,
            CreatedAt = tenant.CreatedAt,
            CreatedBy = tenant.CreatedBy,
            UpdatedAt = tenant.UpdatedAt,
            UpdatedBy = tenant.UpdatedBy,
            RowVersion = Convert.ToBase64String(tenant.RowVersion)
        };
    }

    private static bool TryParseRowVersion(string rowVersion, out byte[] rowVersionBytes)
    {
        rowVersionBytes = Array.Empty<byte>();

        if (string.IsNullOrWhiteSpace(rowVersion))
        {
            return false;
        }

        try
        {
            rowVersionBytes = Convert.FromBase64String(rowVersion);
            return rowVersionBytes.Length > 0;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}
