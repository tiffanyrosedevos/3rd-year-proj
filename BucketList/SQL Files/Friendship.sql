CREATE TABLE [dbo].[Friendship]
(
	[friendInviteID] INT NOT NULL PRIMARY KEY, 
    [userID1] INT NOT NULL, 
    [userID2] INT NOT NULL,
	CONSTRAINT FK_FriendInvite_User1 FOREIGN KEY ([userID1])
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_FriendInvite_User2 FOREIGN KEY ([userID2])
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
)
