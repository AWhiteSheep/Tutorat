CREATE TABLE [dbo].[Utilisateurs]
(
	Identifiant nvarchar(100) NOT NULL PRIMARY KEY,
	NiveauUtilisateur int null,
	Nom nvarchar(max) not null,
	Prénom nvarchar(max) not null,
	Created_Date datetime default GETDATE()
	
	CONSTRAINT FK_Uilisateur_Role FOREIGN KEY (NiveauUtilisateur)
        REFERENCES [Roles] (Id)
)
