namespace vCore.Application.Services;

public interface ITenantRegistrationStatusPolicy
{
    string ResolveInitialStatus();
}
