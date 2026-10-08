# vCore ProductModule API Specification

**Document:** ProductModule.md  
**Version:** 1.0  
**Status:** Batch 1 API Design Baseline  
**Database:** vCore  
**Schema:** `product`  
**Physical Table:** `product.ProductModule`  
**API Version:** v1  
**API Type:** Association API

## 1. Purpose

Defines the API boundary for `product.ProductModule`. The supplied vCore SQL script is authoritative for the physical schema. This is a business-capability API, not a direct table CRUD wrapper.

## 2. Physical Schema Contract

| Column | SQL Type | Nullability |
|---|---|---|
| `ProductModuleId` | `uniqueidentifier` | NOT NULL |
| `ProductId` | `uniqueidentifier` | NOT NULL |
| `ModuleId` | `uniqueidentifier` | NOT NULL |
| `Status` | `varchar(30)` | NOT NULL |
| `IsDefault` | `bit` | NOT NULL |
| `DisplayOrder` | `int` | NOT NULL |
| `CreatedAt` | `datetime2(3)` | NOT NULL |
| `CreatedBy` | `uniqueidentifier` | NULL |
| `UpdatedAt` | `datetime2(3)` | NULL |
| `UpdatedBy` | `uniqueidentifier` | NULL |
| `RowVersion` | `timestamp` | NOT NULL |

### Keys / Unique Constraints

| Constraint | Type | Columns |
|---|---|---|
| `PK_ProductModule` | PRIMARY KEY | ProductModuleId |
| `UQ_ProductModule_Product_Module` | UNIQUE | ProductId, ModuleId |

### Foreign Keys

```sql
ALTER TABLE [product].[ProductModule]  WITH CHECK ADD  CONSTRAINT [FK_ProductModule_Module] FOREIGN KEY([ModuleId])
REFERENCES [product].[Module] ([ModuleId])
```
```sql
ALTER TABLE [product].[ProductModule]  WITH CHECK ADD  CONSTRAINT [FK_ProductModule_Product] FOREIGN KEY([ProductId])
REFERENCES [product].[Product] ([ProductId])
```
```sql
ALTER TABLE [subscription].[PlanModule]  WITH CHECK ADD  CONSTRAINT [FK_PlanModule_ProductModule] FOREIGN KEY([ProductModuleId])
REFERENCES [product].[ProductModule] ([ProductModuleId])
```
```sql
ALTER TABLE [subscription].[SubscriptionModule]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionModule_ProductModule] FOREIGN KEY([ProductModuleId])
REFERENCES [product].[ProductModule] ([ProductModuleId])
```
```sql
ALTER TABLE [tenant].[TenantModule]  WITH CHECK ADD  CONSTRAINT [FK_TenantModule_ProductModule] FOREIGN KEY([ProductModuleId])
REFERENCES [product].[ProductModule] ([ProductModuleId])
```

### Authoritative CREATE TABLE

```sql
CREATE TABLE [product].[ProductModule](
	[ProductModuleId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[ModuleId] [uniqueidentifier] NOT NULL,
	[Status] [varchar](30) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedAt] [datetime2](3) NOT NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime2](3) NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProductModule] PRIMARY KEY CLUSTERED 
(
	[ProductModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_ProductModule_Product_Module] UNIQUE NONCLUSTERED 
(
	[ProductId] ASC,
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

## 3. API Boundary

**Base resource:** `/api/v1/products/{productId}/modules`

Use business operations appropriate to the entity. Association tables (`UserRole`, `RolePermission`, `ProductModule`, `ModuleFeature`) must not automatically become unrestricted CRUD endpoints. They should be managed through the parent business API/assignment operation. `UserSession` is a security/session capability rather than a normal master CRUD resource.

Recommended operations are: create/read/search/update only where supported by business lifecycle rules; destructive delete must require an approved policy.

## 4. DTO Guidelines

Create and update DTOs must contain only client-owned business fields. Primary keys, audit fields, and SQL Server `RowVersion` must be server-managed unless the common vCore API contract explicitly says otherwise. Responses may expose the concurrency token using the standard vCore serialization convention.

## 5. Validation

Validate required fields, SQL length/type limits, unique constraints, foreign-key relationships, and all database CHECK constraints. Business validation may add approved domain rules; Copilot must not invent unsupported requirements.

## 6. Authorization & Tenant Isolation

Use vCore IAM permissions. Tenant-scoped records must use trusted tenant context and must prevent normal users from crossing tenant boundaries. Platform/global records must follow their approved authorization scope.

## 7. Architecture

```text
Controller → Application Service → Domain Rules → Repository → Database
```

Controllers handle HTTP concerns only. Application services orchestrate use cases. Domain code owns invariants. Repositories own persistence.

## 8. Transactions & Concurrency

Use a database transaction when a business operation atomically changes multiple vCore tables. Cross-database operations use APIs/events/orchestration rather than distributed SQL transactions. If `RowVersion` exists, use optimistic concurrency and return the common vCore `409 Conflict` contract for stale updates.

## 9. Error Handling

Use the common vCore error contract. Applicable categories include not found, validation, duplicate business key, invalid relationship, authorization/tenant isolation, lifecycle, and concurrency errors.

## 10. Testing

Test validation, keys, relationships, CHECK/default constraints, business rules, authorization, tenant isolation, concurrency, transaction rollback, and successful/error API responses.

## 11. GitHub Copilot Rules

1. Use the exact SQL schema in this document.
2. Do not invent columns or constraints.
3. Do not expose EF entities directly.
4. Keep controllers thin.
5. Put orchestration in Application Services.
6. Put invariants in Domain.
7. Put persistence in Repository/Infrastructure.
8. Follow common vCore API conventions.
9. Generate tests.
10. Do not create table-level CRUD APIs where the business boundary is an aggregate, assignment, session, or association operation.

## 12. Definition of Done

- [x] Physical schema captured from supplied SQL
- [x] Columns/types/nullability captured
- [x] Keys/unique constraints captured
- [x] Foreign keys captured
- [x] API boundary defined
- [x] DTO strategy defined
- [x] Validation defined
- [x] Authorization/tenant isolation defined
- [x] Transaction/concurrency approach defined
- [x] Testing and Copilot guidance defined
- [ ] Final business lifecycle/permission identifiers approved where applicable

## 13. Source Alignment

Physical database details in this document are derived from the supplied **vCore SQL database script**. Where the SQL does not define a business rule, this specification does not silently invent one.
