CREATE TABLE [dbo].[Notifications]
(
	IdentityKey INT NOT NULL PRIMARY KEY identity(0,3),
	[IdentifiantUtilisateurReceiver] nvarchar(100) not null foreign key references Utilisateurs (Identifiant)
		on delete cascade,
	[IdentifiantUtilisateurRelated] nvarchar(100) null foreign key references Utilisateurs(Identifiant),
	[_IdentityKeyEntityRelated] nvarchar(max),
	[_TypeEntity] nvarchar(max),
	[NotificationName] nvarchar(100) not null,
	[Seen] bit not null default 0,
	[SeenAt] time null,
	[Message] text not null,

	constraint ck_notification_type check (NotificationName IN ('Error', 'Success', 'info', 'Communication')),
	constraint ck_notification_entity_type check ([_TypeEntity] IN ('Demandes', 'Inscriptions'))

)
