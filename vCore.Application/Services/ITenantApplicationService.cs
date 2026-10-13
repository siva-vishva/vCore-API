using vCore.Application.DTOs;

namespace vCore.Application.Services;

public interface ITenantApplicationService
{
    Task<TenantResponse> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken = default);
    Task<TenantResponse?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<PagedResult<TenantResponse>> ListAsync(TenantListRequest request, CancellationToken cancellationToken = default);
    Task<TenantResponse> UpdateAsync(Guid tenantId, UpdateTenantRequest request, CancellationToken cancellationToken = default);
    Task<TenantResponse> ChangeStatusAsync(Guid tenantId, ChangeTenantStatusRequest request, CancellationToken cancellationToken = default);
    Task<ProvisionTenantResponse> ProvisionAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
