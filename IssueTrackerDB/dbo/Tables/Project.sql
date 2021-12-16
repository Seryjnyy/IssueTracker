CREATE TABLE [dbo].[Project] (
    [ProjectId]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50)  NOT NULL,
    [UserID]          NVARCHAR (128) NOT NULL,
    [Description]     NVARCHAR (200) NOT NULL,
    [DateTimeCreated] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC)
);

