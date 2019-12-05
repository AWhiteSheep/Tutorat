CREATE TRIGGER [AfterInsertDemandes]
	ON [dbo].[Demandes]
	after INSERT
	AS
	BEGIN
		SET NOCOUNT ON
		-- declaration des variables
		declare @idDemandeur nvarchar(100);
		declare @idTuteur nvarchar(100);
		declare @nomDemandeur nvarchar(max);
		declare @nomTuteur nvarchar(max);
		declare @entityRelated nvarchar(max);
		
		set @entityRelated = (select h.ServiceId from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey);
		set @idDemandeur = (select i.IdentifiantUtilisateur from inserted i);
		set @idTuteur = (select s.TuteurIdentifiant from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey
											left join [Services] s on s.IdentityKey = h.ServiceId);
		set @nomTuteur = (select (u.Prénom + ' ' + u.Nom) as 'Nom du tuteur' from Utilisateurs u where Identifiant = @idTuteur);
		set @nomDemandeur = (select (u.Prénom + ' ' + u.Nom) as 'Nom de l''élève' from Utilisateurs u where Identifiant = @idDemandeur);

		-- notifier le tuteur qu'il y a une nouvelle demande
		insert into 
			Notifications (
				[IdentifiantUtilisateurReceiver], 
				[IdentifiantUtilisateurRelated], 
				[_IdentityKeyEntityRelated],
				[_TypeEntity],
				[NotificationName], 
				[Message])
			values (
				@idTuteur,
				@idDemandeur,
				@entityRelated,
				'Demandes',
				'info',
				'Vous avez reçu une nouvelle demande d''un de vos paire, '+ @nomDemandeur+ '.')

	END
