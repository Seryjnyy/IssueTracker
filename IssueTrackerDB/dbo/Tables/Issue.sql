CREATE TABLE [dbo].[Issue] (
    [IssueId]          INT            IDENTITY (1, 1) NOT NULL,
    [AuthorID]         NVARCHAR (128) NOT NULL,
    [Description]      NVARCHAR (400) NOT NULL,
    [DateTimeCreated]  DATETIME       NOT NULL,
    [DateTimeDeadline] DATETIME       NULL,
    [Status]           NVARCHAR (50)  NOT NULL,
    [Priority]         NVARCHAR (50)  NOT NULL,
    [AssigneeID]       NVARCHAR (128) NULL,
    [ProjectID]        INT            NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Type]             NVARCHAR (50)  NOT NULL,
    [DateTimeUpdated]  DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([IssueId] ASC),
    CONSTRAINT [FK_IssueForProject] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Project] ([ProjectId])
);

