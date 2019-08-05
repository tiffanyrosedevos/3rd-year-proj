CREATE TABLE [dbo].[SuggestedChallenge]
(
	[suggestedChallengeID] INT NOT NULL PRIMARY KEY, 
    [userID] INT NULL, 
    [title] VARCHAR(50) NULL, 
    [description] VARCHAR(MAX) NULL, 
    [difficulty] INT NULL, 
    [points] INT NULL, 
    [needPhoto] BIT NULL, 
    [canBeGroup] BIT NULL,
	CONSTRAINT FK_SuggestedChallenge_User FOREIGN KEY (userID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
)
