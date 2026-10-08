# vCore API Error Handling

## Centralized Handling

Use centralized exception handling middleware. Controllers should not
contain repetitive generic try/catch logic.

## Error Contract

Use RFC 7807 ProblemDetails or the project-approved equivalent. Include
a stable application error code and correlation/trace identifier where
appropriate.

## Business Errors

Examples: - CUSTOMER_NOT_FOUND - CUSTOMER_DUPLICATE_CODE -
CUSTOMER_INACTIVE - TENANT_NOT_FOUND - TENANT_ACCESS_DENIED -
CONCURRENCY_CONFLICT - VALIDATION_ERROR

## Validation

Return field-level validation details where possible.

## Unexpected Errors

Log the exception with correlation information and return a safe
response. Never expose stack traces or SQL details.

## External Failures

Use timeouts and controlled retry strategies. Avoid indefinite retries.
Return a meaningful service-unavailable response when appropriate.

## Database Errors

Translate known constraint and concurrency failures into controlled
application errors.
