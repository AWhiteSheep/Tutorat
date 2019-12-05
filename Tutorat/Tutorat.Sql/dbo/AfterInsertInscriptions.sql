CREATE TRIGGER AfterInsertInscriptions
ON [Inscriptions]
AFTER Insert 
AS
BEGIN
	-- supprime tous les demandes faient avant.
	Delete a from Demandes a
		join [inserted] i on a.IdentifiantUtilisateur = i.IdentifiantDemandeur
		and a.IdentifiantHoraire = i.IdentifiantHoraire;
END
