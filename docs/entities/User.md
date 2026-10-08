# vCore User API Specification

**Document:** User.md  
**Version:** 1.0  
**Status:** Batch 1 API Design Baseline  
**Database:** vCore  
**Schema:** `iam`  
**Physical Table:** `iam.User`  
**API Version:** v1  
**API Type:** Aggregate API

## 1. Purpose

Defines the API boundary for `iam.User`. The supplied vCore SQL script is authoritative for the physical schema. This is a business-capability API, not a direct table CRUD wrapper.

## 2. Physical Schema Contract

| Column | SQL Type | Nullability |
|---|---|---|
| `UserId` | `uniqueidentifier` | NOT NULL |
| `TenantId` | `uniqueidentifier` | NULL |
| `UserName` | `varchar(100)` | NOT NULL |
| `Email` | `nvarchar(320)` | NOT NULL |
| `FirstName` | `nvarchar(100)` | NOT NULL |
| `LastName` | `nvarchar(100)` | NULL |
| `DisplayName` | `nvarchar(200)` | NOT NULL |
| `Status` | `varchar(30)` | NOT NULL |
| `AuthenticationType` | `varchar(30)` | NOT NULL |
| `LastLoginAt` | `datetime2(3)` | NULL |
| `CreatedAt` | `datetime2(3)` | NOT NULL |
| `CreatedBy` | `uniqueidentifier` | NULL |
| `UpdatedAt` | `datetime2(3)` | NULL |
| `UpdatedBy` | `uniqueidentifier` | NULL |
| `RowVersion` | `timestamp` | NOT NULL |

### Keys / Unique Constraints

| Constraint | Type | Columns |
|---|---|---|
| `PK_User` | PRIMARY KEY | UserId |

### Foreign Keys

```sql
ALTER TABLE [audit].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [audit].[SecurityEvent]  WITH CHECK ADD  CONSTRAINT [FK_SecurityEvent_ResolvedBy] FOREIGN KEY([ResolvedBy])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [audit].[SecurityEvent]  WITH CHECK ADD  CONSTRAINT [FK_SecurityEvent_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [config].[ConfigurationChangeHistory]  WITH CHECK ADD  CONSTRAINT [FK_ConfigurationChangeHistory_User] FOREIGN KEY([ChangedBy])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [deployment].[DeploymentAudit]  WITH CHECK ADD  CONSTRAINT [FK_DeploymentAudit_PerformedBy] FOREIGN KEY([PerformedBy])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [document].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_OwnerUser] FOREIGN KEY([OwnerUserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [document].[DocumentAccess]  WITH CHECK ADD  CONSTRAINT [FK_DocumentAccess_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [iam].[LoginHistory]  WITH CHECK ADD  CONSTRAINT [FK_LoginHistory_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [iam].[MFAConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_MFAConfiguration_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [iam].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Tenant] FOREIGN KEY([TenantId])
REFERENCES [tenant].[Tenant] ([TenantId])
```
```sql
ALTER TABLE [iam].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [iam].[UserSession]  WITH CHECK ADD  CONSTRAINT [FK_UserSession_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [integration].[ApiDefinition]  WITH CHECK ADD  CONSTRAINT [FK_ApiDefinition_OwnerUser] FOREIGN KEY([OwnerUserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [integration].[ApiKey]  WITH CHECK ADD  CONSTRAINT [FK_ApiKey_OwnerUser] FOREIGN KEY([OwnerUserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [integration].[IntegrationError]  WITH CHECK ADD  CONSTRAINT [FK_IntegrationError_ResolvedBy] FOREIGN KEY([ResolvedBy])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [notification].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [notification].[NotificationHistory]  WITH CHECK ADD  CONSTRAINT [FK_NotificationHistory_User] FOREIGN KEY([RecipientUserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [notification].[NotificationPreference]  WITH CHECK ADD  CONSTRAINT [FK_NotificationPreference_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [notification].[NotificationQueue]  WITH CHECK ADD  CONSTRAINT [FK_NotificationQueue_User] FOREIGN KEY([RecipientUserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [reporting].[ReportPermission]  WITH CHECK ADD  CONSTRAINT [FK_ReportPermission_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [search].[SavedSearch]  WITH CHECK ADD  CONSTRAINT [FK_SavedSearch_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [search].[SearchHistory]  WITH CHECK ADD  CONSTRAINT [FK_SearchHistory_User] FOREIGN KEY([UserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [workflow].[ApprovalAction]  WITH CHECK ADD  CONSTRAINT [FK_ApprovalAction_User] FOREIGN KEY([ActionBy])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [workflow].[ApprovalRequest]  WITH CHECK ADD  CONSTRAINT [FK_ApprovalRequest_AssignedTo] FOREIGN KEY([AssignedTo])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [workflow].[ApprovalRequest]  WITH CHECK ADD  CONSTRAINT [FK_ApprovalRequest_RequestedBy] FOREIGN KEY([RequestedBy])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [workflow].[WorkflowEscalation]  WITH CHECK ADD  CONSTRAINT [FK_WorkflowEscalation_User] FOREIGN KEY([EscalateToUserId])
REFERENCES [iam].[User] ([UserId])
```
```sql
ALTER TABLE [workflow].[WorkflowHistory]  WITH CHECK ADD  CONSTRAINT [FK_WorkflowHistory_User] FOREIGN KEY([PerformedBy])
REFERENCES [iam].[User] ([UserId])
```

### Authoritative CREATE TABLE

```sql
CREATE TABLE [iam].[User](
	[UserId] [uniqueidentifier] NOT NULL,
	[TenantId] [uniqueidentifier] NULL,
	[UserName] [varchar](100) NOT NULL,
	[Email] [nvarchar](320) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NULL,
	[DisplayName] [nvarchar](200) NOT NULL,
	[Status] [varchar](30) NOT NULL,
	[AuthenticationType] [varchar](30) NOT NULL,
	[LastLoginAt] [datetime2](3) NULL,
	[CreatedAt] [datetime2](3) NOT NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime2](3) NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

## 3. API Boundary

**Base resource:** `/api/v1/users`

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
