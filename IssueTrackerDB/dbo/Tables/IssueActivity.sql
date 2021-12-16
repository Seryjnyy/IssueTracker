CREATE TABLE [dbo].[IssueActivity] (
    [IssueActivityID] INT            IDENTITY (1, 1) NOT NULL,
    [IssueID]         INT            NOT NULL,
    [UserID]          NVARCHAR (128) NOT NULL,
    [DateTimeCreated] DATETIME       NOT NULL,
    [ActivityContent] NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([IssueActivityID] ASC),
    CONSTRAINT [FK_IssueActivity_ToIssue] FOREIGN KEY ([IssueID]) REFERENCES [dbo].[Issue] ([IssueId]) ON DELETE CASCADE
);

