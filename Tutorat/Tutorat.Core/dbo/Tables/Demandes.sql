CREATE TABLE [dbo].[Demandes] (
    [IdentifiantUtilisateur] NVARCHAR (450) NOT NULL,
    [IdentifiantHoraire]     INT            NOT NULL,
    [DateCreated]            DATE           CONSTRAINT [DF__Demandes__DateCr__2F10007B] DEFAULT (getdate()) NOT NULL,
    [DateExpired]            DATE           CONSTRAINT [DF__Demandes__DateEx__300424B4] DEFAULT (dateadd(day,(1),getdate())) NOT NULL,
    [Notified]               BIT            CONSTRAINT [DF__Demandes__Notifi__30F848ED] DEFAULT ((0)) NULL,
    [NotificationId] INT NULL, 
    CONSTRAINT [CK__Demandes__3F466844] CHECK (datediff(day,[DateCreated],[DateExpired])>(0)),
    CONSTRAINT [FK__Demandes__Identi__37A5467C] FOREIGN KEY ([IdentifiantUtilisateur]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK__Demandes__Identifianthoraire] FOREIGN KEY ([IdentifiantHoraire]) REFERENCES [dbo].[Horraire] ([IdentityKey]),
    CONSTRAINT [FK__Demandes__Notification] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notifications] ([IdentityKey]),
    CONSTRAINT [PK__Demandes] PRIMARY KEY ([IdentifiantUtilisateur], [IdentifiantHoraire])
);