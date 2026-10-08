# vCore API Security

## Authentication

Protected APIs require authentication using the project-approved
identity mechanism.

## Authorization

Use permission/policy-based authorization. vCore contains User, Role,
Permission, UserRole and RolePermission concepts. Endpoints must require
appropriate permissions.

## Tenant Isolation

Tenant context must come from the authenticated security context
wherever possible. Validate resource ownership. Never permit
cross-tenant access.

## Input Security

Validate required fields, length, format, allowed values and
identifiers. Never concatenate user input into SQL.

## Secrets

Never hard-code credentials, tokens, API keys or secrets. Use approved
secret/configuration management.

## Sensitive Data

Do not return or log unnecessary sensitive information.

## External Integrations

Use authenticated service-to-service communication. vFinance and other
products access vCore through approved APIs/events, not direct SQL.

## Audit

Use the existing vCore audit/security foundation for important
operations.

## Default

New endpoints are protected by default. Anonymous access requires
explicit approval.
