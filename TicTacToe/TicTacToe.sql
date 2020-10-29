IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [UserModel] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [IsEmailConfirmed] bit NOT NULL,
    [EmailConfirmationDate] datetime2 NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_UserModel] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [GameInvitationModel] (
    [Id] uniqueidentifier NOT NULL,
    [EmailTo] nvarchar(max) NULL,
    [InvitedBy] nvarchar(max) NULL,
    [InvitedByUserId] uniqueidentifier NULL,
    [InvitedById] uniqueidentifier NOT NULL,
    [IsConfirmed] bit NOT NULL,
    [ConfirmationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_GameInvitationModel] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GameInvitationModel_UserModel_InvitedByUserId] FOREIGN KEY ([InvitedByUserId]) REFERENCES [UserModel] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [GameSessionModel] (
    [Id] uniqueidentifier NOT NULL,
    [UserId1] uniqueidentifier NOT NULL,
    [UserId2] uniqueidentifier NOT NULL,
    [User1Id] uniqueidentifier NULL,
    [WinnerId] uniqueidentifier NOT NULL,
    [ActiveUserID] uniqueidentifier NOT NULL,
    [TurnFinished] bit NOT NULL,
    [TurnNumber] int NOT NULL,
    CONSTRAINT [PK_GameSessionModel] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GameSessionModel_UserModel_User1Id] FOREIGN KEY ([User1Id]) REFERENCES [UserModel] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_GameSessionModel_UserModel_UserId2] FOREIGN KEY ([UserId2]) REFERENCES [UserModel] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TurnModel] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [X] int NOT NULL,
    [Y] int NOT NULL,
    [Email] nvarchar(max) NULL,
    [IconNumber] nvarchar(max) NULL,
    [GameSessionModelId] uniqueidentifier NULL,
    CONSTRAINT [PK_TurnModel] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TurnModel_GameSessionModel_GameSessionModelId] FOREIGN KEY ([GameSessionModelId]) REFERENCES [GameSessionModel] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TurnModel_UserModel_UserId] FOREIGN KEY ([UserId]) REFERENCES [UserModel] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_GameInvitationModel_InvitedByUserId] ON [GameInvitationModel] ([InvitedByUserId]);

GO

CREATE INDEX [IX_GameSessionModel_User1Id] ON [GameSessionModel] ([User1Id]);

GO

CREATE INDEX [IX_GameSessionModel_UserId2] ON [GameSessionModel] ([UserId2]);

GO

CREATE INDEX [IX_TurnModel_GameSessionModelId] ON [TurnModel] ([GameSessionModelId]);

GO

CREATE INDEX [IX_TurnModel_UserId] ON [TurnModel] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200623021041_InitialDbSchema', N'3.1.5');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserModel]') AND [c].[name] = N'IsEmailConfirmed');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UserModel] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [UserModel] DROP COLUMN [IsEmailConfirmed];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserModel]') AND [c].[name] = N'Email');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [UserModel] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [UserModel] ALTER COLUMN [Email] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [AccessFailedCount] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [UserModel] ADD [ConcurrencyStamp] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [EmailConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [UserModel] ADD [LockoutEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [UserModel] ADD [LockoutEnd] datetimeoffset NULL;

GO

ALTER TABLE [UserModel] ADD [NormalizedEmail] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [NormalizedUserName] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [PasswordHash] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [PhoneNumber] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [PhoneNumberConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [UserModel] ADD [SecurityStamp] nvarchar(max) NULL;

GO

ALTER TABLE [UserModel] ADD [TwoFactorEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [UserModel] ADD [UserName] nvarchar(max) NULL;

GO

CREATE TABLE [RoleModel] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleModel] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_GameSessionModel_WinnerId] ON [GameSessionModel] ([WinnerId]);

GO

ALTER TABLE [GameSessionModel] ADD CONSTRAINT [FK_GameSessionModel_UserModel_WinnerId] FOREIGN KEY ([WinnerId]) REFERENCES [UserModel] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200625012240_IdentityDb', N'3.1.5');

GO

CREATE TABLE [FBUserModel] (
    [Id] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_FBUserModel] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629132553_FBUser', N'3.1.5');

GO

ALTER TABLE [UserModel] ADD [FbUserId] uniqueidentifier NULL;

GO

CREATE INDEX [IX_UserModel_FbUserId] ON [UserModel] ([FbUserId]);

GO

ALTER TABLE [UserModel] ADD CONSTRAINT [FK_UserModel_FBUserModel_FbUserId] FOREIGN KEY ([FbUserId]) REFERENCES [FBUserModel] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629135551_FBUser_New', N'3.1.5');

GO

ALTER TABLE [UserModel] DROP CONSTRAINT [FK_UserModel_FBUserModel_FbUserId];

GO

DROP TABLE [FBUserModel];

GO

DROP INDEX [IX_UserModel_FbUserId] ON [UserModel];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserModel]') AND [c].[name] = N'FbUserId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [UserModel] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [UserModel] DROP COLUMN [FbUserId];

GO

CREATE TABLE [IdentityUserRole<Guid>] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_IdentityUserRole<Guid>] PRIMARY KEY ([UserId], [RoleId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629140242_FBUser_New1', N'3.1.5');

GO

DROP TABLE [IdentityUserRole<Guid>];

GO

CREATE TABLE [IdentityUserLogin<Guid>] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    CONSTRAINT [PK_IdentityUserLogin<Guid>] PRIMARY KEY ([UserId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629143359_FBUser_New2', N'3.1.5');

GO

DROP TABLE [IdentityUserLogin<Guid>];

GO

CREATE TABLE [FBUserModel] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [Id] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_FBUserModel] PRIMARY KEY ([UserId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629144139_FBUser_New3', N'3.1.5');

GO

DROP TABLE [FBUserModel];

GO

CREATE TABLE [IdentityUserLogin<Guid>] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    CONSTRAINT [PK_IdentityUserLogin<Guid>] PRIMARY KEY ([UserId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629144416_FBUser_New4', N'3.1.5');

GO

CREATE TABLE [TwoFactorCodeModel] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [TokenProvider] nvarchar(max) NULL,
    [TokenCode] nvarchar(max) NULL,
    CONSTRAINT [PK_TwoFactorCodeModel] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TwoFactorCodeModel_UserModel_UserId] FOREIGN KEY ([UserId]) REFERENCES [UserModel] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_TwoFactorCodeModel_UserId] ON [TwoFactorCodeModel] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200701030250_AddTwoFactorCode', N'3.1.5');

GO

CREATE TABLE [UserRoleModel] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserRoleModel] PRIMARY KEY ([UserId], [RoleId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200703052029_IdentityDb2', N'3.1.5');

GO

