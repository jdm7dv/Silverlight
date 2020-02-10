-- --------------------------------------------------
-- Date Created: 09/27/2009 23:33:59
-- Generated from EDMX file: C:\Users\7600\Documents\Visual Studio 10\Projects\WebServices\DataLayer\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [Test]
GO
IF SCHEMA_ID('dbo') IS NULL EXECUTE('CREATE SCHEMA [dbo]')
GO

-- --------------------------------------------------
-- Dropping existing FK constraints
-- --------------------------------------------------

IF OBJECT_ID('dbo.FK_EntryCategory_Entry', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntryCategory] DROP CONSTRAINT [FK_EntryCategory_Entry]
GO
IF OBJECT_ID('dbo.FK_EntryCategory_Category', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntryCategory] DROP CONSTRAINT [FK_EntryCategory_Category]
GO
IF OBJECT_ID('dbo.FK_CommentEntry', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_CommentEntry]
GO
IF OBJECT_ID('dbo.FK_EntryAuthor', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entries] DROP CONSTRAINT [FK_EntryAuthor]
GO
IF OBJECT_ID('dbo.FK_AuthorRole_Author', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuthorRole] DROP CONSTRAINT [FK_AuthorRole_Author]
GO
IF OBJECT_ID('dbo.FK_AuthorRole_Role', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuthorRole] DROP CONSTRAINT [FK_AuthorRole_Role]
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID('dbo.Entries', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entries];
GO
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID('dbo.Comments', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID('dbo.Authors', 'U') IS NOT NULL
    DROP TABLE [dbo].[Authors];
GO
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID('dbo.EntryCategory', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntryCategory];
GO
IF OBJECT_ID('dbo.AuthorRole', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuthorRole];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Entries'
CREATE TABLE [dbo].[Entries] (
    [EntryId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Created] datetime  NULL,
    [Rating] decimal(29,29)  NULL,
    [Published] bit  NULL,
    [CommentsEnabled] bit  NULL,
    [Modified] datetime  NULL,
    [Author_AuthorId] int  NULL
);
GO
-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO
-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [CommentId] int IDENTITY(1,1) NOT NULL,
    [CommentText] nvarchar(max)  NULL,
    [Created] datetime  NULL,
    [Name] nvarchar(max)  NULL,
    [Entry_EntryId] int  NOT NULL
);
GO
-- Creating table 'Authors'
CREATE TABLE [dbo].[Authors] (
    [AuthorId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO
-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO
-- Creating table 'EntryCategory'
CREATE TABLE [dbo].[EntryCategory] (
    [Entries_EntryId] int  NOT NULL,
    [Categories_CategoryId] int  NOT NULL
);
GO
-- Creating table 'AuthorRole'
CREATE TABLE [dbo].[AuthorRole] (
    [Authors_AuthorId] int  NOT NULL,
    [Roles_RoleId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all Primary Key Constraints
-- --------------------------------------------------

-- Creating primary key on [EntryId] in table 'Entries'
ALTER TABLE [dbo].[Entries] WITH NOCHECK
ADD CONSTRAINT [PK_Entries]
    PRIMARY KEY CLUSTERED ([EntryId] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [CategoryId] in table 'Categories'
ALTER TABLE [dbo].[Categories] WITH NOCHECK
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [CommentId] in table 'Comments'
ALTER TABLE [dbo].[Comments] WITH NOCHECK
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([CommentId] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [AuthorId] in table 'Authors'
ALTER TABLE [dbo].[Authors] WITH NOCHECK
ADD CONSTRAINT [PK_Authors]
    PRIMARY KEY CLUSTERED ([AuthorId] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles] WITH NOCHECK
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Entries_EntryId], [Categories_CategoryId] in table 'EntryCategory'
ALTER TABLE [dbo].[EntryCategory] WITH NOCHECK
ADD CONSTRAINT [PK_EntryCategory]
    PRIMARY KEY NONCLUSTERED ([Entries_EntryId], [Categories_CategoryId] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Authors_AuthorId], [Roles_RoleId] in table 'AuthorRole'
ALTER TABLE [dbo].[AuthorRole] WITH NOCHECK
ADD CONSTRAINT [PK_AuthorRole]
    PRIMARY KEY NONCLUSTERED ([Authors_AuthorId], [Roles_RoleId] ASC)
    ON [PRIMARY]
GO

-- --------------------------------------------------
-- Creating all Foreign Key Constraints
-- --------------------------------------------------

-- Creating foreign key on [Entries_EntryId] in table 'EntryCategory'
ALTER TABLE [dbo].[EntryCategory] WITH NOCHECK
ADD CONSTRAINT [FK_EntryCategory_Entry]
    FOREIGN KEY ([Entries_EntryId])
    REFERENCES [dbo].[Entries]
        ([EntryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Categories_CategoryId] in table 'EntryCategory'
ALTER TABLE [dbo].[EntryCategory] WITH NOCHECK
ADD CONSTRAINT [FK_EntryCategory_Category]
    FOREIGN KEY ([Categories_CategoryId])
    REFERENCES [dbo].[Categories]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Entry_EntryId] in table 'Comments'
ALTER TABLE [dbo].[Comments] WITH NOCHECK
ADD CONSTRAINT [FK_CommentEntry]
    FOREIGN KEY ([Entry_EntryId])
    REFERENCES [dbo].[Entries]
        ([EntryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Author_AuthorId] in table 'Entries'
ALTER TABLE [dbo].[Entries] WITH NOCHECK
ADD CONSTRAINT [FK_EntryAuthor]
    FOREIGN KEY ([Author_AuthorId])
    REFERENCES [dbo].[Authors]
        ([AuthorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Authors_AuthorId] in table 'AuthorRole'
ALTER TABLE [dbo].[AuthorRole] WITH NOCHECK
ADD CONSTRAINT [FK_AuthorRole_Author]
    FOREIGN KEY ([Authors_AuthorId])
    REFERENCES [dbo].[Authors]
        ([AuthorId])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Roles_RoleId] in table 'AuthorRole'
ALTER TABLE [dbo].[AuthorRole] WITH NOCHECK
ADD CONSTRAINT [FK_AuthorRole_Role]
    FOREIGN KEY ([Roles_RoleId])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

