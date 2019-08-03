CREATE TABLE [dbo].[GroupChallengeInvite]
(
	[challengeInviteID] INT NOT NULL PRIMARY KEY, 
    [fromUserID] INT NOT NULL, 
    [toUserID] INT NOT NULL, 
    [challengeID] INT NOT NULL, 
    [status] VARCHAR(50) NULL,
	CONSTRAINT FK_GroupChallengeInvite_UserChallenge FOREIGN KEY (fromUserID, challengeID)
        REFERENCES [dbo].[UserChallenge] (userID, challengeID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_GroupChallengeInvite_User FOREIGN KEY (toUserID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
)

