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


GO
CREATE TRIGGER [AfterInsertDemandes]
	ON dbo.Demandes
	after INSERT
	AS
	BEGIN
		SET NOCOUNT ON
		-- declaration des variables
		declare @idDemandeur nvarchar(450);
		declare @idTuteur nvarchar(450);
		declare @nomDemandeur nvarchar(max);
		declare @nomTuteur nvarchar(max);
		declare @entityRelated nvarchar(max);
		declare @notificationId int;
		
		set @entityRelated = (select h.ServiceId from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey);
		set @idDemandeur = (select i.IdentifiantUtilisateur from inserted i);
		set @idTuteur = (select s.TuteurId from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey
											left join [Services] s on s.IdentityKey = h.ServiceId);
		set @nomTuteur = (select (u.UserName) as 'Numéro du tuteur' from AspNetUsers u where Id = @idTuteur);
		set @nomDemandeur = (select (u.UserName) as 'Nom de l''élève' from AspNetUsers u where Id = @idDemandeur);

		-- notifier le tuteur qu'il y a une nouvelle demande
		insert into 
			Notifications (
				[IdentifiantUtilisateurReceiver],
				[IdentifiantUtilisateurRelated],
				[NotificationName],
				[Message])
			values (
				@idTuteur,
				@idDemandeur,
				'Demandes',
				'Vous avez reçu une nouvelle demande d''un de vos paire.');

		set @notificationId = (select top 1 IdentityKey from Notifications);
		
	    update Demandes
		set Demandes.NotificationId = @notificationId from inserted
		where Demandes.IdentifiantUtilisateur = inserted.IdentifiantUtilisateur and Demandes.IdentifiantHoraire = inserted.IdentifiantHoraire
			   
	END
