# vCore OrganizationUnit API Specification

**Document:** OrganizationUnit.md  
**Version:** 1.0  
**Status:** Batch 1 API Design Baseline  
**Database:** vCore  
**Schema:** `org`  
**Physical Table:** `org.OrganizationUnit`  
**API Version:** v1  
**API Type:** Aggregate/Entity API

## 1. Purpose

Defines the API boundary for `org.OrganizationUnit`. The supplied vCore SQL script is authoritative for the physical schema. This is a business-capability API, not a direct table CRUD wrapper.

## 2. Physical Schema Contract

| Column | SQL Type | Nullability |
|---|---|---|
| `OrganizationUnitId` | `uniqueidentifier` | NOT NULL |
| `TenantId` | `uniqueidentifier` | NOT NULL |
| `OrganizationId` | `uniqueidentifier` | NOT NULL |
| `ParentOrganizationUnitId` | `uniqueidentifier` | NULL |
| `LocationId` | `uniqueidentifier` | NULL |
| `UnitCode` | `varchar(50)` | NOT NULL |
| `UnitName` | `nvarchar(200)` | NOT NULL |
| `UnitType` | `varchar(50)` | NULL |
| `Description` | `nvarchar(1000)` | NULL |
| `Status` | `varchar(30)` | NOT NULL |
| `DisplayOrder` | `int` | NOT NULL |
| `CreatedAt` | `datetime2(3)` | NOT NULL |
| `CreatedBy` | `uniqueidentifier` | NULL |
| `UpdatedAt` | `datetime2(3)` | NULL |
| `UpdatedBy` | `uniqueidentifier` | NULL |
| `RowVersion` | `timestamp` | NOT NULL |

### Keys / Unique Constraints

| Constraint | Type | Columns |
|---|---|---|
| `PK_OrganizationUnit` | PRIMARY KEY | OrganizationUnitId |

### Foreign Keys

```sql
ALTER TABLE [org].[CostCenter]  WITH CHECK ADD  CONSTRAINT [FK_CostCenter_OrganizationUnit] FOREIGN KEY([OrganizationUnitId])
REFERENCES [org].[OrganizationUnit] ([OrganizationUnitId])
```
```sql
ALTER TABLE [org].[OrganizationUnit]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUnit_Location] FOREIGN KEY([LocationId])
REFERENCES [org].[Location] ([LocationId])
```
```sql
ALTER TABLE [org].[OrganizationUnit]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUnit_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [org].[Organization] ([OrganizationId])
```
```sql
ALTER TABLE [org].[OrganizationUnit]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUnit_Parent] FOREIGN KEY([ParentOrganizationUnitId])
REFERENCES [org].[OrganizationUnit] ([OrganizationUnitId])
```
```sql
ALTER TABLE [org].[OrganizationUnit]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUnit_Tenant] FOREIGN KEY([TenantId])
REFERENCES [tenant].[Tenant] ([TenantId])
```

### Authoritative CREATE TABLE

```sql
CREATE TABLE [org].[OrganizationUnit](
	[OrganizationUnitId] [uniqueidentifier] NOT NULL,
	[TenantId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[ParentOrganizationUnitId] [uniqueidentifier] NULL,
	[LocationId] [uniqueidentifier] NULL,
	[UnitCode] [varchar](50) NOT NULL,
	[UnitName] [nvarchar](200) NOT NULL,
	[UnitType] [varchar](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[Status] [varchar](30) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedAt] [datetime2](3) NOT NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime2](3) NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_OrganizationUnit] PRIMARY KEY CLUSTERED 
(
	[OrganizationUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

## 3. API Boundary

**Base resource:** `/api/v1/organization-units`

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
