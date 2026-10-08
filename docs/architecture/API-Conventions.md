# vCore API Conventions

## URL

Use `/api/v1/{plural-resource}`.

Examples: - `/api/v1/customers` - `/api/v1/products` - `/api/v1/tenants`

## HTTP Methods

-   GET: retrieve
-   POST: create or execute a business operation
-   PUT: full replacement where appropriate
-   PATCH: partial update where supported
-   DELETE: only when deletion/deactivation is explicitly appropriate

## Responses

Use DTOs. Do not serialize EF entities directly.

## Collections

Use a consistent pagination strategy for potentially large collections.

## Filtering and Sorting

Use explicit validated query parameters. Never accept arbitrary SQL
fragments.

## Status Codes

Use standard HTTP semantics: 200, 201, 202, 204, 400, 401, 403, 404,
409, 422, 429, 500 and 503 where appropriate.

## Idempotency

Operations that can create duplicate business effects on retry must use
an idempotency strategy where required.

## Naming

Use clear business names: CustomerController, CustomerService,
CreateCustomerRequest, CustomerResponse.

## Contract Changes

Classify changes as non-breaking, potentially breaking or breaking.
Breaking changes require versioning/compatibility planning.

## Query vs Command

GET is for retrieval. State-changing operations must not be hidden
behind GET.

## Principle

Design APIs around what consumers need to accomplish, not around the
number of database tables.
