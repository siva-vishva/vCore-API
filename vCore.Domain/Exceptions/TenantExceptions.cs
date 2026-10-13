namespace vCore.Domain.Exceptions;

public abstract class TenantException : Exception
{
    protected TenantException(string message) : base(message)
    {
    }
}

public sealed class TenantNotFoundException : TenantException
{
    public TenantNotFoundException(Guid tenantId)
        : base($"Tenant '{tenantId}' was not found.")
    {
    }
}

public sealed class TenantCodeAlreadyExistsException : TenantException
{
    public TenantCodeAlreadyExistsException(string tenantCode)
        : base($"Tenant code '{tenantCode}' already exists.")
    {
    }
}

public sealed class TenantValidationException : TenantException
{
    public TenantValidationException(string message)
        : base(message)
    {
    }
}

public sealed class TenantConcurrencyException : TenantException
{
    public TenantConcurrencyException(string message)
        : base(message)
    {
    }
}

public sealed class TenantAccessDeniedException : TenantException
{
    public TenantAccessDeniedException(string message)
        : base(message)
    {
    }
}
