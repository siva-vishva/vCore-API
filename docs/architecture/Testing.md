# vCore API Testing Strategy

## Test Layers

-   Unit tests
-   Domain/application tests
-   Integration tests
-   API/endpoint tests
-   Database tests where necessary

## Unit Tests

Test business rules, domain behavior, validators and application
decisions without requiring a real SQL Server unless intentionally
classified as integration testing.

## Integration Tests

Test EF mappings, database constraints, transactions, repositories and
important authorization/tenant paths.

## API Tests

Test status codes, validation, response contracts, authorization, tenant
isolation and error responses.

## Change Process

When functionality changes: 1. Update the specification. 2. Identify
affected tests. 3. Add/update tests. 4. Implement. 5. Run relevant
tests. 6. Run the full suite before release.

## AI-Generated Code

Copilot-generated code is not complete until it has appropriate tests
and passes review.
