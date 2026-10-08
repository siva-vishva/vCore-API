# vCore Business Logic Guidelines

## Principle

Business logic must be separated from HTTP and persistence.

Preferred flow:

Controller → Application Service → Domain Rules →
Repository/Infrastructure

## Controller

Handles HTTP request/response concerns only. It must not contain core
business rules.

## Application Service

Orchestrates use cases such as CreateCustomer, UpdateCustomer,
ActivateCustomer and DeactivateCustomer. It may coordinate repositories,
domain rules, transactions and events.

## Domain

Contains business invariants, policies, state transitions, calculations
and domain events. Keep it independent of HTTP and EF Core.

## Repository

Handles persistence. Repositories must not become the primary location
for business decisions.

## External Integrations

External APIs, message brokers, storage and other integrations belong in
Infrastructure/application integration components.

## Business Operations Over CRUD

If an operation has meaningful business behavior, expose the business
operation rather than forcing clients to perform multiple low-level CRUD
calls.
