CREATE TABLE [dbo].[ProjectActivity] (
    [ProjectActivityId] INT            IDENTITY (1, 1) NOT NULL,
    [ProjectID]         INT            NOT NULL,
    [UserID]            NVARCHAR (128) NOT NULL,
    [DateTimeCreated]   DATETIME       NOT NULL,
    [ActivityContent]   NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectActivityId] ASC)
);
