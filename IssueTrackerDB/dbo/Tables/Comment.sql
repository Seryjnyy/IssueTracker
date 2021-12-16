CREATE TABLE [dbo].[Comment] (
    [CommentID]       INT            IDENTITY (1, 1) NOT NULL,
    [AuthorID]        NVARCHAR (128) NOT NULL,
    [Content]         NVARCHAR (400) NOT NULL,
    [DateTimeCreated] DATETIME       NOT NULL,
    [IssueID]         INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentID] ASC),
    CONSTRAINT [FK_Comment_ToIssue] FOREIGN KEY ([IssueID]) REFERENCES [dbo].[Issue] ([IssueId]) ON DELETE CASCADE
);

