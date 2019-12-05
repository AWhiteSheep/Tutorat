CREATE TABLE [dbo].[Inscriptions]
(
	IdentifiantDemandeur nvarchar(100) not null 
		foreign key references [Utilisateurs] (Identifiant) on delete cascade,
	IdentifiantHoraire int not null 
		foreign key references [Horraire] (IdentityKey) on delete cascade,
	[AcceptedDate] datetime not null default getdate(),
	[EndDate] datetime not null default Dateadd(month, 3, getdate()),

	constraint key_user_scheduler 
		primary key (IdentifiantDemandeur ASC, IdentifiantHoraire ASC)
)
