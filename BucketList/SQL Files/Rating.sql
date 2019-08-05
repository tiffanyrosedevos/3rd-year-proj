CREATE TABLE [dbo].[Rating]
(
	[ratingID] INT NOT NULL PRIMARY KEY, 
    [challengeId] INT NOT NULL, 
    [userID] INT NOT NULL, 
    [rating] INT NOT NULL, 
    [review] VARCHAR(MAX) NULL,
	CONSTRAINT FK_Rating_Challenge FOREIGN KEY (challengeID)
        REFERENCES [dbo].[Challenge] (challengeID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_Rating_User FOREIGN KEY (userID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
)
