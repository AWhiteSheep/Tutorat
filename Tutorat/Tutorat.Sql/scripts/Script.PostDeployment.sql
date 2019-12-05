/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
-- ROLES
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Admin')
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Tuteur')
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Eleve')

GO
-- UTILISATEUR
INSERT INTO [dbo].[Utilisateurs] ([Identifiant], [NiveauUtilisateur], [Nom], [Prénom], [Created_Date]) VALUES ('1111111', 1, 'Caron', 'Sylveste', '2019-12-05 11:48:19')
INSERT INTO [dbo].[Utilisateurs] ([Identifiant], [NiveauUtilisateur], [Nom], [Prénom], [Created_Date]) VALUES ('1473192', 0, 'Routhier', 'Yan ', '2019-12-05 11:48:01')
INSERT INTO [dbo].[Utilisateurs] ([Identifiant], [NiveauUtilisateur], [Nom], [Prénom], [Created_Date]) VALUES ('2222222', 2, 'Obama', 'Barack', '2019-12-05 11:48:54')

GO
-- SERVICES
INSERT INTO [dbo].[Services] ([TuteurIdentifiant], [Titre], [Description]) VALUES ('1473192', 'Mathématique', 'Un bon mathématicien')
INSERT INTO [dbo].[Services] ([TuteurIdentifiant], [Titre], [Description]) VALUES ('2222222', 'Francais', 'J''enseigne du francais pour ceux qui ont de la difficultés')
