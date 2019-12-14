CREATE TABLE [dbo].[Horraire] (
    [IdentityKey]         INT      IDENTITY (0, 1) NOT NULL,
    [ServiceId]           INT      NOT NULL,
    [Jour]                INT      NOT NULL,
    [HeureDebut]          TIME (7) NOT NULL,
    [NbHeure]             INT      NOT NULL,
    [NbMinute]            INT      DEFAULT ((0)) NOT NULL,
    [EleveMaxInscription] INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([IdentityKey] ASC),
    CHECK ([jour]<(7) AND [jour]>(-1)),
    CONSTRAINT [FK__Horraire__Servic__38996AB5] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([IdentityKey]) ON DELETE CASCADE
);

