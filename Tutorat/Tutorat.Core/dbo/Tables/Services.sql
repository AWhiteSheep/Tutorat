CREATE TABLE [dbo].[Services] (
    [IdentityKey] INT            IDENTITY (0, 1) NOT NULL,
    [TuteurId]    NVARCHAR (450) NOT NULL,
    [Titre]       NVARCHAR (MAX) NOT NULL,
    [Description] TEXT           NOT NULL,
    CONSTRAINT [PK__Services__796424B83E787365] PRIMARY KEY CLUSTERED ([IdentityKey] ASC),
    CONSTRAINT [FK_Services_AspNetUsers] FOREIGN KEY ([TuteurId]) REFERENCES [dbo].[AspNetUsers] ([Id]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[Services] NOCHECK CONSTRAINT [FK_Services_AspNetUsers];

