CREATE TABLE [dbo].[UserAchievement]
(
	[userID] INT NOT NULL, 
    [achievementID] INT NOT NULL,
	CONSTRAINT FK_UserAchievement_User FOREIGN KEY (userID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_UserAchievement_Achievement FOREIGN KEY (achievementID)
        REFERENCES [dbo].[Achievement] (achievementID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	PRIMARY KEY(userID, achievementID)
)
