CREATE TABLE [dbo].[UserChallenge]
(
	[userID] INT NOT NULL, 
    [challengeID] INT NOT NULL,
	[status] VARCHAR(50) NULL, 
    [photo] VARCHAR(50) NULL, 
    CONSTRAINT FK_UserChallenge_User FOREIGN KEY (userID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_UserChallenge_Challenge FOREIGN KEY (challengeID)
        REFERENCES [dbo].[Challenge] (challengeID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	PRIMARY KEY(userID, challengeID)
)
