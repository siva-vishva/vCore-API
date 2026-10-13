using System.Security.Claims;
using vCore.Application.Services;

namespace vCore.Api.Security;

public sealed class HttpContextAuthenticatedUserContext : IAuthenticatedTenantUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAuthenticatedUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is null || !user.Identity?.IsAuthenticated == true)
        {
            return null;
        }

        var value = user.FindFirst("sub")?.Value
            ?? user.FindFirst("user_id")?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(value, out var userId) ? userId : null;
    }
}
