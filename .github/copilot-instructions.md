# vCore GitHub Copilot Instructions

## Project Context

This repository implements the vCore .NET Web API platform for the
vSuite ecosystem.

vCore is the shared platform and master-data foundation consumed by
vFinance, vCommerce, vWorkforce and vLogistics.

vCore owns shared master/platform data. Business transactions belong to
the respective product domains.

## Mandatory Reading

Before generating or modifying code, read: 1.
`/docs/architecture/API-Architecture.md` 2.
`/docs/architecture/API-Conventions.md` 3.
`/docs/architecture/Security.md` 4.
`/docs/architecture/Error-Handling.md` 5.
`/docs/architecture/Validation.md` 6.
`/docs/architecture/Transactions.md` 7.
`/docs/architecture/Business-Logic-Guidelines.md` 8.
`/docs/architecture/Testing.md` 9. The relevant entity specification.
10. The relevant aggregate/API specification when available.

Do not invent requirements silently.

## Database Rules

-   Use the approved vCore database schema and table definitions.
-   Do not create tables unless explicitly requested.
-   Do not rename tables/columns without explicit approval.
-   Respect existing PK/FK/unique/check constraints.
-   Respect CreatedAt, CreatedBy, UpdatedAt, UpdatedBy and RowVersion
    conventions.
-   Do not expose internal supporting tables as public APIs unless
    explicitly specified.

## API Rules

-   Build RESTful business-capability APIs.
-   Use `/api/v1/...` unless otherwise specified.
-   Use DTOs; never expose EF entities directly.
-   Use async I/O and CancellationToken.
-   Use consistent HTTP status codes and ProblemDetails.
-   Apply authorization and tenant isolation.
-   Use pagination for large collections.
-   Do not create an endpoint simply because a table exists.

## Aggregate Rules

One API operation may read/write multiple tables. Treat the business
operation as the API boundary and the database tables as implementation
details.

When multiple tables must be written atomically: - Validate the complete
request. - Apply business rules. - Use one transaction. - Commit only
when all required operations succeed. - Roll back on failure. - Publish
relevant events after successful persistence.

## Business Logic

Do not put business logic in controllers.

Use: - Application services for use-case orchestration. - Domain
entities/policies/services for core business rules. -
Repositories/infrastructure for persistence.

Do not put business rules into repositories merely because database
access is convenient.

## Validation

Use request validators for input validation. Use application/domain
rules for business validation. Do not rely solely on application
validation for database integrity.

## Security

Never: - Hard-code credentials or secrets. - Log passwords, access
tokens or API keys. - Bypass authorization. - Bypass tenant isolation. -
Give other products direct SQL access to vCore.

vFinance and other products must consume vCore through APIs/events.

## Cross-Database Integration

When another vSuite product needs vCore master data: - Use APIs for
synchronous validation/retrieval. - Use events for change propagation. -
Use reference/read models where appropriate. - Do not create duplicate
authoritative master records.

## Change Management

When asked to add, remove or change functionality: 1. Read the relevant
specifications. 2. Identify impacted entities, aggregates, APIs,
business rules and tests. 3. Update the specification first when the
intended behavior has changed. 4. Perform impact analysis. 5. Modify
only affected implementation. 6. Update/add tests. 7. Preserve backward
compatibility unless a breaking change is explicitly approved. 8. Do not
modify unrelated code.

The Markdown specifications define intended behavior. Code implements
it.

## Do Not Guess

If a required rule, relationship, field, API contract or security rule
is not defined: - Do not invent it silently. - Identify the missing
requirement. - Prefer the approved database/reconciliation specification
when authoritative. - Ask for clarification only when safe
implementation is otherwise impossible.

## Existing Code First

Before creating a new service, repository, middleware, validator or
helper: 1. Search for an existing implementation. 2. Reuse or extend it
when appropriate. 3. Avoid duplicate infrastructure.

## API Stability

Treat existing public APIs as contracts. Do not remove or rename
contract fields without explicit approval. Use versioning for breaking
changes.

## Code Quality

Generated code must: - Compile. - Follow the existing solution
structure. - Use dependency injection. - Use clear naming. - Avoid
unnecessary abstractions. - Avoid duplicated logic. - Handle
cancellation. - Handle errors consistently. - Include appropriate tests.

## Preferred Copilot Workflow

For a new API: 1. Read architecture instructions. 2. Read entity
specification. 3. Read aggregate/API specification. 4. Identify related
tables. 5. Generate DTOs. 6. Generate validators. 7. Generate
application service/use case. 8. Generate domain rules where required.
9. Generate repository/infrastructure code. 10. Generate controller. 11.
Generate tests. 12. Review. 13. Build and test.

For a change: 1. Update the relevant Markdown specification first. 2.
Ask Copilot for impact analysis. 3. Modify implementation. 4. Update
tests. 5. Run tests. 6. Review the diff. 7. Commit specification and
implementation together where appropriate.

## Final Principle

Copilot is an implementation assistant, not the system architect.

Follow the documented architecture, database baseline, entity
specifications, aggregate specifications and business rules. If
specifications and implementation disagree, identify the discrepancy
instead of silently choosing one.
