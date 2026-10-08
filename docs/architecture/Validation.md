# vCore API Validation

## Validation Layers

1.  Request/input validation
2.  Application/business validation
3.  Domain invariants
4.  Database constraints

## Request Validation

Use FluentValidation or the approved mechanism for required fields,
formats, lengths, ranges and allowed values.

## Business Validation

Business rules belong in Application/Domain layers. Examples: -
CustomerCode unique within tenant - Tenant must be active - Business
customer requires required regulatory information - Resource must belong
to the authorized tenant

## Domain Invariants

Rules that must always be true for a domain entity should be enforced by
the domain model/policies.

## Database Integrity

Database constraints remain responsible for PKs, FKs, unique
constraints, checks, required columns and defaults. Application
validation does not replace database constraints.

## Cross-Entity Validation

Validate required relationships before commit. Use a transaction for
same-database writes and APIs/events for cross-database relationships.
