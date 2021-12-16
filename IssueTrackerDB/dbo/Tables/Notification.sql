CREATE TABLE [dbo].[Notification] (
    [NotificationID]  INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          NVARCHAR (128) NOT NULL,
    [AuthorID]        NVARCHAR (128) NULL,
    [ProjectID]       INT            NULL,
    [IssueID]         INT            NULL,
    [Content]         NVARCHAR (200) NOT NULL,
    [DateTimeCreated] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([NotificationID] ASC),
    CONSTRAINT [FK_Notification_ToProject] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_Notification_ToIssue] FOREIGN KEY ([IssueID]) REFERENCES [dbo].[Issue] ([IssueId])
);

