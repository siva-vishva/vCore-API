# vCore API Architecture

## Purpose

Defines the architectural standards for the vCore .NET Web API platform.

vCore is the shared platform and master-data foundation consumed by
vSuite products such as vFinance, vCommerce, vWorkforce and vLogistics.

## Technology

-   ASP.NET Core Web API
-   Project-approved current .NET LTS version
-   Entity Framework Core
-   SQL Server
-   OpenAPI / Swagger
-   Built-in dependency injection
-   async/await and CancellationToken

## Architecture

Preferred flow:

API → Application → Domain → Infrastructure → SQL Server

### API Layer

Handles HTTP, routing, authentication/authorization integration, request
binding, response mapping, versioning and OpenAPI metadata. It must not
contain core business rules or direct database logic.

### Application Layer

Handles use-case orchestration, DTOs, commands/queries where
appropriate, validation, transaction coordination and event publication.

### Domain Layer

Contains core business rules, entities, value objects, policies,
invariants and domain events. It must not depend on ASP.NET Core, EF
Core, SQL Server or HTTP.

### Infrastructure Layer

Contains EF Core, entity configurations, repositories where justified,
external integrations, messaging, storage and caching.

## Business-Capability-First APIs

One database table does not automatically equal one API.

A single business operation may read/write multiple tables. Example:
creating a Customer may involve Customer, Address, Contact and
relationship tables and should be handled as one business operation when
atomicity is required.

## API Types

-   Entity APIs for simple resources
-   Aggregate APIs for operations spanning related entities
-   Query APIs for business-oriented read models combining multiple
    tables

## Database Boundary

vCore API owns access to the vCore database. Other products must not
directly query vCore tables. Use APIs for synchronous access and events
for state propagation.

## Multi-Tenancy

Tenant isolation is mandatory. Validate that requested resources belong
to the authenticated/authorized tenant. Never trust a client-supplied
TenantId without validation.

## DTO Boundary

Never expose EF Core entities directly. Use explicit request/response
DTOs.

## Transactions

A business operation modifying multiple vCore tables should normally use
one database transaction when atomicity is required. Cross-database
processes should normally use APIs, events and orchestration rather than
distributed SQL transactions.

## Concurrency

Use RowVersion for optimistic concurrency where supported. Stale updates
must return a controlled conflict.

## Events

Publish events for important state changes, such as CustomerCreated,
CustomerUpdated, CustomerDeactivated, ProductCreated and ProductUpdated.
Use an outbox pattern where reliable publication is required.

## API Versioning

Use `/api/v1/...`. Breaking changes require an approved versioning
strategy.

## Observability

Support structured logging, correlation IDs, tracing where available,
metrics and health checks. Never log passwords, secrets or access
tokens.

## Documentation

Each API capability should have entity, aggregate/business-rule and API
specifications plus OpenAPI documentation.

## AI-Assisted Development

Copilot must follow these architecture rules and the relevant
specifications. Before changes: read the architecture,
entity/aggregate/API specifications, identify impact, implement the
minimum change, update tests, build and review.

## Core Principle

vCore APIs are business-capability APIs over a shared platform database.
The database schema is an implementation boundary, not automatically the
API boundary.
