CREATE TABLE [dbo].[Rating] (
    [ratingID]    INT           IDENTITY (1, 1) NOT NULL,
    [challengeID] INT           NOT NULL,
    [userID]      INT           NOT NULL,
    [rating]      INT           NOT NULL,
    [review]      VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ratingID] ASC),
    CONSTRAINT [FK_Rating_Challenge] FOREIGN KEY ([challengeId]) REFERENCES [dbo].[Challenge] ([challengeID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Rating_User] FOREIGN KEY ([userID]) REFERENCES [dbo].[User] ([userID]) ON DELETE CASCADE ON UPDATE CASCADE
);

