CREATE TRIGGER [ForInsertInscriptions]
	ON [dbo].Inscriptions
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
