CREATE TABLE [dbo].[ProjectUser]
(
	[ProjectUserId] INT NOT NULL PRIMARY KEY, 
    [ProjectID] INT NOT NULL, 
    [UserID] NVARCHAR(128) NOT NULL, 
    [Role] INT NOT NULL
)
