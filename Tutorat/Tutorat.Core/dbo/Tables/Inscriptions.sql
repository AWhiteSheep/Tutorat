CREATE TABLE [dbo].[Inscriptions] (
    [IdentifiantDemandeur] NVARCHAR (450) NOT NULL,
    [IdentifiantHoraire]   INT            NOT NULL,
    [AcceptedDate]         DATETIME       CONSTRAINT [DF__Inscripti__Accep__32E0915F] DEFAULT (getdate()) NOT NULL,
    [EndDate]              DATETIME       CONSTRAINT [DF__Inscripti__EndDa__33D4B598] DEFAULT (dateadd(month,(3),getdate())) NOT NULL,
    [NotificationId] int null,
	CONSTRAINT [FK__Inscription__Notification] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notifications] ([IdentityKey]),
	CONSTRAINT [FK__Inscripti__Ident__3A81B327] FOREIGN KEY ([IdentifiantDemandeur]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [PK__Inscriptions] PRIMARY KEY ([IdentifiantDemandeur], [IdentifiantHoraire])
);