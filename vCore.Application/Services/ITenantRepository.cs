using vCore.Application.DTOs;
using vCore.Domain.Tenants;

namespace vCore.Application.Services;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Tenant?> GetByCodeAsync(string tenantCode, CancellationToken cancellationToken = default);
    Task<PagedResult<Tenant>> ListAsync(TenantListRequest query, CancellationToken cancellationToken = default);
    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default);
}
