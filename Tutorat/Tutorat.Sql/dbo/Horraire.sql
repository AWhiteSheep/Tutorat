CREATE TABLE [dbo].[Horraire]
(
	[IdentityKey] int NOT NULL PRIMARY KEY identity(0,1),
	[ServiceId] int not null foreign key references [Services] (IdentityKey) on delete cascade,
	[Jour] int not null check (jour < 7 and jour > -1),
	[HeureDebut] time not null,
	[NHours] int not null,
	[NMin] int not null,
	[EleveMaxInscription] int not null
)
