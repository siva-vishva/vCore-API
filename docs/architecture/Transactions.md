# vCore Transaction and Consistency Guidelines

## Transaction Boundary

A transaction should represent a meaningful business operation.

## Multi-Table Operations

When one business operation writes multiple vCore tables, use one
database transaction where atomicity is required.

Example Customer creation may include Customer, Address, Contact and
relationship tables. Required writes commit or roll back together.

## Concurrency

Use RowVersion for optimistic concurrency. Stale updates must return a
controlled conflict.

## Cross-Database Operations

Do not use distributed SQL transactions as the default between vCore and
product databases. Prefer APIs, events and orchestration/Saga patterns.

## Event Reliability

Do not publish a success event before the corresponding database
transaction is safely committed. Use a transactional outbox where
reliable event publication is required.

## Idempotency

Retryable operations must avoid duplicate business effects using
idempotency keys or appropriate business uniqueness constraints.

## Principle

Never partially commit a business operation when the specification
requires atomicity.
