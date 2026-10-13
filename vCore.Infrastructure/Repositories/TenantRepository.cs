using Microsoft.EntityFrameworkCore;
using vCore.Application.DTOs;
using vCore.Application.Services;
using vCore.Domain.Tenants;
using vCore.Infrastructure.Persistence;

namespace vCore.Infrastructure.Repositories;

public sealed class TenantRepository : ITenantRepository
{
    private readonly VCoreDbContext _dbContext;

    public TenantRepository(VCoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tenants.FirstOrDefaultAsync(x => x.TenantId == tenantId, cancellationToken);
    }

    public async Task<Tenant?> GetByCodeAsync(string tenantCode, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tenants.FirstOrDefaultAsync(x => x.TenantCode == tenantCode, cancellationToken);
    }

    public async Task<PagedResult<Tenant>> ListAsync(TenantListRequest query, CancellationToken cancellationToken = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 25 : query.PageSize;

        var items = _dbContext.Tenants.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            items = items.Where(x => x.Status == query.Status);
        }

        if (!string.IsNullOrWhiteSpace(query.TenantType))
        {
            items = items.Where(x => x.TenantType == query.TenantType);
        }

        if (!string.IsNullOrWhiteSpace(query.TenantCode))
        {
            items = items.Where(x => x.TenantCode.Contains(query.TenantCode));
        }

        if (!string.IsNullOrWhiteSpace(query.TenantName))
        {
            items = items.Where(x => x.TenantName.Contains(query.TenantName));
        }

        if (!string.IsNullOrWhiteSpace(query.CountryCode))
        {
            items = items.Where(x => x.CountryCode == query.CountryCode);
        }

        var totalCount = await items.CountAsync(cancellationToken);

        items = ApplySort(items, query.Sort);

        var pageItems = await items
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Tenant>
        {
            Items = pageItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _dbContext.Tenants.Update(tenant);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<Tenant> ApplySort(IQueryable<Tenant> query, string? sort)
    {
        return sort?.Trim().ToLowerInvariant() switch
        {
            "tenantcode" => query.OrderBy(x => x.TenantCode),
            "tenantname" => query.OrderBy(x => x.TenantName),
            "status" => query.OrderBy(x => x.Status),
            "tenanttype" => query.OrderBy(x => x.TenantType),
            "countrycode" => query.OrderBy(x => x.CountryCode),
            _ => query.OrderBy(x => x.TenantName)
        };
    }
}
