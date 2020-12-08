IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [B_Course] (
    [IndentityId] int NOT NULL IDENTITY,
    [Id] bigint NOT NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    [Name] nvarchar(200) NULL,
    CONSTRAINT [PK_B_Course] PRIMARY KEY ([IndentityId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201029091815_init', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course]') AND [c].[name] = N'Id');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [B_Course] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [B_Course] ALTER COLUMN [Id] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201102063415_setIdTyp', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course]') AND [c].[name] = N'Id');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [B_Course] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [B_Course] ALTER COLUMN [Id] nvarchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201102063556_SetIdLen', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[B_Course].[Id]', N'ID', N'COLUMN';
GO

ALTER TABLE [B_Course] ADD [CatalogId] nvarchar(200) NULL;
GO

ALTER TABLE [B_Course] ADD [CatalogName] nvarchar(300) NULL;
GO

ALTER TABLE [B_Course] ADD [CoverUrl] nvarchar(500) NULL;
GO

ALTER TABLE [B_Course] ADD [Creator] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course] ADD [CreatorName] nvarchar(100) NULL;
GO

ALTER TABLE [B_Course] ADD [Goal] nvarchar(max) NULL;
GO

ALTER TABLE [B_Course] ADD [Intro] nvarchar(max) NULL;
GO

ALTER TABLE [B_Course] ADD [RegionId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course] ADD [RegionName] nvarchar(100) NULL;
GO

ALTER TABLE [B_Course] ADD [SchoolName] nvarchar(100) NULL;
GO

ALTER TABLE [B_Course] ADD [Schoolid] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course] ADD [SignatureId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course] ADD [SignatureName] nvarchar(200) NULL;
GO

ALTER TABLE [B_Course] ADD [Status] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_B_Course_Deleted] ON [B_Course] ([Deleted]);
GO

CREATE INDEX [IX_B_Course_ID] ON [B_Course] ([ID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201105023058_tags', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [B_Course_Tags] (
    [IndentityId] int NOT NULL IDENTITY,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    [CourseId] nvarchar(50) NULL,
    [SchoolId] int NOT NULL,
    [RegtionId] int NOT NULL,
    [Creator] int NOT NULL,
    [AssetId] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [TypeName] nvarchar(max) NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_B_Course_Tags] PRIMARY KEY ([IndentityId])
);
GO

CREATE INDEX [IX_B_Course_Tags_Deleted] ON [B_Course_Tags] ([Deleted]);
GO

CREATE INDEX [IX_B_Course_Tags_ID] ON [B_Course_Tags] ([ID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201105023349_tagEntity', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course]') AND [c].[name] = N'Creator');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [B_Course] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [B_Course] DROP COLUMN [Creator];
GO

EXEC sp_rename N'[B_Course].[Schoolid]', N'SchoolId', N'COLUMN';
GO

ALTER TABLE [B_Course] ADD [CreatorId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201105030556_updatefeild', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course]') AND [c].[name] = N'CreatorId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [B_Course] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [B_Course] DROP COLUMN [CreatorId];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course]') AND [c].[name] = N'RegionId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [B_Course] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [B_Course] DROP COLUMN [RegionId];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course]') AND [c].[name] = N'SchoolId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [B_Course] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [B_Course] DROP COLUMN [SchoolId];
GO

ALTER TABLE [B_Course] ADD [CreatorCode] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course] ADD [RegionCode] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course] ADD [SchoolCode] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201105031332_updatefeild_2', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201105033341_tags_catgoryid', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course_Tags] ADD [CategoryId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201105033526_tags_catgoryid_2', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [U_PlatformUser] (
    [IndentityId] int NOT NULL IDENTITY,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    [UserId] int NOT NULL,
    [SchoolId] int NOT NULL,
    [SectionId] int NOT NULL,
    [Name] nvarchar(100) NULL,
    [CourseShelves] int NOT NULL,
    [StdJoined] int NOT NULL,
    CONSTRAINT [PK_U_PlatformUser] PRIMARY KEY ([IndentityId])
);
GO

CREATE INDEX [IX_U_PlatformUser_Deleted] ON [U_PlatformUser] ([Deleted]);
GO

CREATE INDEX [IX_U_PlatformUser_ID] ON [U_PlatformUser] ([ID]);
GO

CREATE INDEX [IX_U_PlatformUser_UserId] ON [U_PlatformUser] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201107024130_platformuser', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [B_Course_DS] (
    [IndentityId] int NOT NULL IDENTITY,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    [OperatorId] int NOT NULL,
    [DsId] uniqueidentifier NOT NULL,
    [SortVal] int NOT NULL,
    [CatalogId] int NOT NULL,
    [IsOpen] bit NOT NULL,
    [CourseId] nvarchar(450) NULL,
    CONSTRAINT [PK_B_Course_DS] PRIMARY KEY ([IndentityId])
);
GO

CREATE INDEX [IX_B_Course_DS_CatalogId] ON [B_Course_DS] ([CatalogId]);
GO

CREATE INDEX [IX_B_Course_DS_CourseId] ON [B_Course_DS] ([CourseId]);
GO

CREATE INDEX [IX_B_Course_DS_Deleted] ON [B_Course_DS] ([Deleted]);
GO

CREATE INDEX [IX_B_Course_DS_ID] ON [B_Course_DS] ([ID]);
GO

CREATE INDEX [IX_B_Course_DS_OperatorId] ON [B_Course_DS] ([OperatorId]);
GO

CREATE INDEX [IX_B_Course_DS_SortVal] ON [B_Course_DS] ([SortVal]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201109085550_quoteds', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_B_Course_DS_CourseId] ON [B_Course_DS];
DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course_DS]') AND [c].[name] = N'CourseId');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [B_Course_DS] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [B_Course_DS] ALTER COLUMN [CourseId] nvarchar(50) NULL;
CREATE INDEX [IX_B_Course_DS_CourseId] ON [B_Course_DS] ([CourseId]);
GO

ALTER TABLE [B_Course_DS] ADD [DsName] nvarchar(500) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201109093917_quoteds_name', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201109094811_quoteds_name1', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course_DS] ADD [OperatorName] nvarchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201110022846_quoteds_operatorName', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [B_Course_Collabrator] (
    [IndentityId] int NOT NULL IDENTITY,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    [CouserId] nvarchar(100) NULL,
    [CollabratorId] int NOT NULL,
    CONSTRAINT [PK_B_Course_Collabrator] PRIMARY KEY ([IndentityId])
);
GO

CREATE INDEX [IX_B_Course_Collabrator_CollabratorId] ON [B_Course_Collabrator] ([CollabratorId]);
GO

CREATE INDEX [IX_B_Course_Collabrator_CouserId] ON [B_Course_Collabrator] ([CouserId]);
GO

CREATE INDEX [IX_B_Course_Collabrator_Deleted] ON [B_Course_Collabrator] ([Deleted]);
GO

CREATE INDEX [IX_B_Course_Collabrator_ID] ON [B_Course_Collabrator] ([ID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201110081649_collabrator', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course_DS] ADD [Cover] nvarchar(500) NULL;
GO

ALTER TABLE [B_Course] ADD [CollbratorCount] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_B_Course_CollbratorCount] ON [B_Course] ([CollbratorCount]);
GO

CREATE INDEX [IX_B_Course_CreatorCode] ON [B_Course] ([CreatorCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201110100234_collabratorCount', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course_Tags]') AND [c].[name] = N'TypeName');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [B_Course_Tags] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [B_Course_Tags] ALTER COLUMN [TypeName] nvarchar(50) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course_Tags]') AND [c].[name] = N'Name');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [B_Course_Tags] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [B_Course_Tags] ALTER COLUMN [Name] nvarchar(50) NULL;
GO

ALTER TABLE [B_Course_Tags] ADD [RegionName] nvarchar(100) NULL;
GO

ALTER TABLE [B_Course_Tags] ADD [SchoolName] nvarchar(100) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201111101814_tag_org_name', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201112083329_UserJoin', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [U_platformUser_Course] (
    [IndentityId] int NOT NULL IDENTITY,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    [PlatUserId] nvarchar(100) NULL,
    [UserId] int NOT NULL,
    [CourseId] nvarchar(100) NULL,
    CONSTRAINT [PK_U_platformUser_Course] PRIMARY KEY ([IndentityId])
);
GO

CREATE INDEX [IX_U_platformUser_Course_CourseId] ON [U_platformUser_Course] ([CourseId]);
GO

CREATE INDEX [IX_U_platformUser_Course_Deleted] ON [U_platformUser_Course] ([Deleted]);
GO

CREATE INDEX [IX_U_platformUser_Course_ID] ON [U_platformUser_Course] ([ID]);
GO

CREATE INDEX [IX_U_platformUser_Course_PlatUserId] ON [U_platformUser_Course] ([PlatUserId]);
GO

CREATE INDEX [IX_U_platformUser_Course_UserId] ON [U_platformUser_Course] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201112084254_UserJoin2', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course] ADD [LeanerCount] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201112091514_CourseLeaner', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP INDEX [IX_U_platformUser_Course_CourseId] ON [U_platformUser_Course];
GO

DROP INDEX [IX_U_platformUser_Course_Deleted] ON [U_platformUser_Course];
GO

DROP INDEX [IX_U_platformUser_Course_ID] ON [U_platformUser_Course];
GO

DROP INDEX [IX_U_platformUser_Course_PlatUserId] ON [U_platformUser_Course];
GO

DROP INDEX [IX_U_platformUser_Course_UserId] ON [U_platformUser_Course];
GO

DROP INDEX [IX_U_PlatformUser_Deleted] ON [U_PlatformUser];
GO

DROP INDEX [IX_U_PlatformUser_ID] ON [U_PlatformUser];
GO

DROP INDEX [IX_U_PlatformUser_UserId] ON [U_PlatformUser];
GO

DROP INDEX [IX_B_Course_Tags_Deleted] ON [B_Course_Tags];
GO

DROP INDEX [IX_B_Course_Tags_ID] ON [B_Course_Tags];
GO

DROP INDEX [IX_B_Course_DS_CatalogId] ON [B_Course_DS];
GO

DROP INDEX [IX_B_Course_DS_CourseId] ON [B_Course_DS];
GO

DROP INDEX [IX_B_Course_DS_Deleted] ON [B_Course_DS];
GO

DROP INDEX [IX_B_Course_DS_ID] ON [B_Course_DS];
GO

DROP INDEX [IX_B_Course_DS_OperatorId] ON [B_Course_DS];
GO

DROP INDEX [IX_B_Course_DS_SortVal] ON [B_Course_DS];
GO

DROP INDEX [IX_B_Course_Collabrator_CollabratorId] ON [B_Course_Collabrator];
GO

DROP INDEX [IX_B_Course_Collabrator_CouserId] ON [B_Course_Collabrator];
GO

DROP INDEX [IX_B_Course_Collabrator_Deleted] ON [B_Course_Collabrator];
GO

DROP INDEX [IX_B_Course_Collabrator_ID] ON [B_Course_Collabrator];
GO

DROP INDEX [IX_B_Course_CollbratorCount] ON [B_Course];
GO

DROP INDEX [IX_B_Course_CreatorCode] ON [B_Course];
GO

DROP INDEX [IX_B_Course_Deleted] ON [B_Course];
GO

DROP INDEX [IX_B_Course_ID] ON [B_Course];
GO

CREATE TABLE [B_SystemConfig] (
    [IndentityId] int NOT NULL IDENTITY,
    [TagAttr] nvarchar(1000) NULL,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_B_SystemConfig] PRIMARY KEY ([IndentityId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201116030748_SystemConfig', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201116103223_AuditLog', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [B_Course_Audit_Log] (
    [IndentityId] int NOT NULL IDENTITY,
    [CouserId] nvarchar(20) NULL,
    [ReviewerName] nvarchar(20) NULL,
    [ReviewerId] int NOT NULL,
    [Desc] nvarchar(1000) NULL,
    [ReviewerOrgId] int NOT NULL,
    [ReviewerOrgName] nvarchar(20) NULL,
    [StatusDesc] nvarchar(100) NULL,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_B_Course_Audit_Log] PRIMARY KEY ([IndentityId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201116103422_AuditLog2', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course] ADD [RegionStatus] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201117054522_RegionStatus', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201118075048_ListedCourse', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [B_Course_Listed] (
    [IndentityId] int NOT NULL IDENTITY,
    [CourseId] nvarchar(50) NULL,
    [OrgId] int NOT NULL,
    [Type] int NOT NULL,
    [OrgName] nvarchar(50) NULL,
    [PltId] nvarchar(50) NULL,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_B_Course_Listed] PRIMARY KEY ([IndentityId])
);
GO

CREATE TABLE [B_Partner] (
    [IndentityId] int NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [CourseCount] int NOT NULL,
    [Name] nvarchar(100) NULL,
    [ResourceId] int NOT NULL,
    [ParentId] int NOT NULL,
    [ID] nvarchar(100) NULL,
    [Deleted] int NOT NULL,
    [CreationTime] datetime2 NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_B_Partner] PRIMARY KEY ([IndentityId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201118075252_ListedCourse2', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Partner] DROP CONSTRAINT [PK_B_Partner];
GO

EXEC sp_rename N'[B_Course_Audit_Log].[CouserId]', N'CourseId', N'COLUMN';
GO

ALTER TABLE [B_Partner] ADD CONSTRAINT [PK_B_Partner] PRIMARY KEY ([ResourceId], [Type]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201119011842_updateLogField', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[B_Course_Collabrator]') AND [c].[name] = N'CouserId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [B_Course_Collabrator] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [B_Course_Collabrator] DROP COLUMN [CouserId];
GO

EXEC sp_rename N'[B_Course_Collabrator].[CollabratorId]', N'RootId', N'COLUMN';
GO

ALTER TABLE [B_Course_Collabrator] ADD [CourseId] nvarchar(max) NULL;
GO

ALTER TABLE [B_Course_Collabrator] ADD [ObjId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course_Collabrator] ADD [ObjName] nvarchar(50) NULL;
GO

ALTER TABLE [B_Course_Collabrator] ADD [OrgId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course_Collabrator] ADD [OrgName] nvarchar(50) NULL;
GO

ALTER TABLE [B_Course_Collabrator] ADD [RootName] nvarchar(50) NULL;
GO

ALTER TABLE [B_Course_Collabrator] ADD [Type] nvarchar(50) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201119092104_CollabratorUpdate', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course_Listed] ADD [Creator] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [B_Course_Listed] ADD [ParentId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201120084943_ListedCreator', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [U_PlatformUser] ADD [CourseCount] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201123055647_UserCount', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course] ADD [Section] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201126020301_CourseSection', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course_DS] ADD [IsShared] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201127094450_CourseDsShared', N'5.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [B_Course_DS] ADD [IsOriginal] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201127100018_CourseDsIsOriginal', N'5.0.0');
GO

COMMIT;
GO

