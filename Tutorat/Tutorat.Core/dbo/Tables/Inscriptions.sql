CREATE TABLE [dbo].[Inscriptions] (
    [IdentifiantDemandeur] NVARCHAR (450) NOT NULL,
    [IdentifiantHoraire]   INT            NOT NULL,
    [AcceptedDate]         DATETIME       CONSTRAINT [DF__Inscripti__Accep__32E0915F] DEFAULT (getdate()) NOT NULL,
    [EndDate]              DATETIME       CONSTRAINT [DF__Inscripti__EndDa__33D4B598] DEFAULT (dateadd(month,(3),getdate())) NOT NULL,
    CONSTRAINT [FK__Inscripti__Ident__3A81B327] FOREIGN KEY ([IdentifiantDemandeur]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [PK__Inscriptions] PRIMARY KEY ([IdentifiantDemandeur], [IdentifiantHoraire])
);


GO
CREATE TRIGGER [ForInsertInscriptions]
	ON dbo.Inscriptions
	FOR INSERT
	AS
	BEGIN
		SET NOCOUNT ON;

	    Declare @nbInscrit int;
		Declare @nbMaxInscription int;
		
		set @nbMaxInscription = (select h.EleveMaxInscription from Horraire h inner join inserted on inserted.IdentifiantHoraire = h.IdentityKey)
		set @nbInscrit = (select count(*) from Inscriptions i left join Horraire h on h.IdentityKey = i.IdentifiantHoraire where h.IdentityKey = (select inserted.IdentifiantHoraire from inserted))

		if(@nbMaxInscription < @nbInscrit)
			begin
				RAISERROR ('Nombre maximal d''inscription atteinte.', 12, 12) 
				rollback transaction
				return
			end		
	END

GO
CREATE TRIGGER AfterInsertInscriptions
ON dbo.Inscriptions
AFTER Insert 
AS
BEGIN
	-- declaration des variables
	declare @identifiantDemandeur nvarchar(450);
	declare @identifiantTuteur nvarchar(450);
	declare @nomTuteur nvarchar(max);
	declare @nomDemandeur nvarchar(max);
	declare @entityRelated nvarchar(max);
		
	set @entityRelated = (select h.ServiceId from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey);
	set @identifiantDemandeur = (select IdentifiantDemandeur from inserted);
	set @identifiantTuteur = (select s.TuteurId from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey
										left join [Services] s on s.IdentityKey = h.ServiceId);
	set @nomTuteur = (select (u.UserName) as 'Numéro du tuteur' from AspNetUsers u where Id = @identifiantTuteur);
	set @nomDemandeur = (select (u.UserName) as 'Numéro de l''élève' from AspNetUsers u where Id = @identifiantDemandeur);

	-- supprime tous les demandes faient avant.
	Delete a from Demandes a
		join [inserted] i on a.IdentifiantUtilisateur = i.IdentifiantDemandeur
		and a.IdentifiantHoraire = i.IdentifiantHoraire;

	-- send notification to user related
	insert into 
		Notifications (
			[IdentifiantUtilisateurReceiver], 
			[IdentifiantUtilisateurRelated], 
			[_IdentityKeyEntityRelated],
			[_TypeEntity],
			[NotificationName], 
			[Message])
		values (
			@identifiantDemandeur,
			@identifiantTuteur,
			@entityRelated,
			'Inscriptions',
			'Success',
			'Vous êtes maintenant inscrit à l''aide par vos paire donné par '+ @nomTuteur+'.')
	insert into 
		Notifications (
			[IdentifiantUtilisateurReceiver], 
			[IdentifiantUtilisateurRelated], 
			[_IdentityKeyEntityRelated],
			[_TypeEntity],
			[NotificationName], 
			[Message])
		values (
			@identifiantTuteur,
			@identifiantDemandeur,
			@entityRelated,
			'Inscriptions',
			'Success',
			@nomDemandeur + ' est inscrit(e) à votre service de tutorat.')

END
