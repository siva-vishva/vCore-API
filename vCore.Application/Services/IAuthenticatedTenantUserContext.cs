namespace vCore.Application.Services;

public interface IAuthenticatedTenantUserContext
{
    Guid? GetCurrentUserId();
}
