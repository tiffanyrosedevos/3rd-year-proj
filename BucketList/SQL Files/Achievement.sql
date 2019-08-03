CREATE TABLE [dbo].[Achievement]
(
	[achievementID] INT NOT NULL PRIMARY KEY, 
    [description] VARCHAR(MAX) NULL, 
    [difficulty] INT NULL, 
    [nrNeeded] INT NULL
)
