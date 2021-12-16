CREATE TABLE [dbo].[ProjectUser] (
    [ProjectUserId] INT            IDENTITY (1, 1) NOT NULL,
    [ProjectID]     INT            NOT NULL,
    [UserID]        NVARCHAR (128) NOT NULL,
    [Role]          NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectUserId] ASC),
    CONSTRAINT [FK_ProjectUser_ToProject] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Project] ([ProjectId])
);

