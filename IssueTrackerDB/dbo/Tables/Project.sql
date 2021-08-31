CREATE TABLE [dbo].[Project]
(
	[ProjectId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Creator] NVARCHAR(128) NOT NULL, 
    [Description] NCHAR(200) NOT NULL, 
    [DateCreated] DATETIME NOT NULL
)
