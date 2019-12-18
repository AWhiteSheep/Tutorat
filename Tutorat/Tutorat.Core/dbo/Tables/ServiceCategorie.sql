CREATE TABLE [dbo].[ServiceCategorie]
(
	ServiceId INT NOT NULL foreign key references [Services] (IdentityKey),
	CategoryId INT NOT NULL foreign key references Categories (Id),
	Constraint PK_SERVICE_CATEGORIE primary key (ServiceId, CategoryId)
)
