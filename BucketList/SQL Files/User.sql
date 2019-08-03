CREATE TABLE [dbo].[User]
(
	[userID] INT NOT NULL PRIMARY KEY, 
    [username] VARCHAR(50) NULL, 
    [password] VARCHAR(50) NULL, 
    [firstName] VARCHAR(50) NULL, 
    [surname] VARCHAR(50) NULL, 
    [email] VARCHAR(50) NULL, 
    [userType] BIT NULL, 
    [points] INT NULL
)
