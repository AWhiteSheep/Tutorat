CREATE TABLE [dbo].[Demandes]
(
	IdentifiantUtilisateur nvarchar(100) not null foreign key references [Utilisateurs] (Identifiant),
	IdentifiantHoraire int not null foreign key references Horraire (IdentityKey),
	DateCreated date not null default getdate(),
	DateExpired date not null default(dateadd(day, 1, getdate())) check (Datediff(day, DateCreated, DateExpired) > 0),

	constraint key_demandes_user_service primary key(IdentifiantUtilisateur ASC, IdentifiantHoraire ASC)
)
