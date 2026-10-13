using vCore.Application.Services;
using vCore.Domain.Tenants;

namespace vCore.Infrastructure.Authorization;

public sealed class DefaultTenantRegistrationStatusPolicy : ITenantRegistrationStatusPolicy
{
    private readonly string? _configuredStatus;

    public DefaultTenantRegistrationStatusPolicy(string? configuredStatus = null)
    {
        _configuredStatus = configuredStatus;
    }

    public string ResolveInitialStatus()
    {
        if (!string.IsNullOrWhiteSpace(_configuredStatus))
        {
            if (!Tenant.Statuses.AllowedValues.Contains(_configuredStatus, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Configured initial tenant status '{_configuredStatus}' is not valid for the approved database constraints.");
            }

            return _configuredStatus;
        }

        throw new InvalidOperationException("Tenant registration status is undefined. Configure an approved business policy before creating a tenant.");
    }
}
