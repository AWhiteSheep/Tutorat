CREATE TABLE [dbo].[Services]
(
	IdentityKey int NOT NULL primary key identity(0,1),
	TuteurIdentifiant nvarchar(100) not null foreign key references Utilisateurs (Identifiant),
	Titre nvarchar(max) not null,
	[Description] text not null
)
