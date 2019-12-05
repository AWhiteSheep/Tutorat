CREATE TRIGGER AfterInsertInscriptions
ON [Inscriptions]
AFTER Insert 
AS
BEGIN
	-- declaration des variables
	declare @identifiantDemandeur nvarchar(100);
	declare @identifiantTuteur nvarchar(100);
	declare @nomTuteur nvarchar(max);
	declare @nomDemandeur nvarchar(max);
	declare @entityRelated nvarchar(max);
		
	set @entityRelated = (select h.ServiceId from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey);
	set @identifiantDemandeur = (select IdentifiantDemandeur from inserted);
	set @identifiantTuteur = (select s.TuteurIdentifiant from inserted i left join Horraire h on i.IdentifiantHoraire = h.IdentityKey
										left join [Services] s on s.IdentityKey = h.ServiceId);
	set @nomTuteur = (select (u.Prénom + ' ' + u.Nom) as 'Nom du tuteur' from Utilisateurs u where Identifiant = @identifiantTuteur);
	set @nomDemandeur = (select (u.Prénom + ' ' + u.Nom) as 'Nom de l''élève' from Utilisateurs u where Identifiant = @identifiantDemandeur);

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
