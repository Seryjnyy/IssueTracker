CREATE TABLE [dbo].[Issues]
(
	[IssueId] INT NOT NULL PRIMARY KEY, 
    [AuthorID] VARCHAR(128) NOT NULL, 
    [Description] NCHAR(400) NOT NULL, 
    [TimeCreated] DATETIME NOT NULL, 
    [Deadline] DATETIME NOT NULL, 
    [Label] INT NOT NULL, 
    [Priority] INT NOT NULL, 
    [AssigneeID] VARCHAR(128) NOT NULL, 
    [ProjectID] INT NOT NULL
)
