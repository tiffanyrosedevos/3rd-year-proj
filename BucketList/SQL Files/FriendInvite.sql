CREATE TABLE [dbo].[FriendInvite]
(
	[friendInviteID] INT NOT NULL PRIMARY KEY, 
    [fromUserID] INT NOT NULL, 
    [toUserID] INT NOT NULL,
	CONSTRAINT FK_FriendInvite_User1 FOREIGN KEY (fromUserID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_FriendInvite_User2 FOREIGN KEY (toUserID)
        REFERENCES [dbo].[User] (userID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
)