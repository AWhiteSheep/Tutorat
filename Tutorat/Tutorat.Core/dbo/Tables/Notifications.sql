CREATE TABLE [dbo].[Notifications] (
    [IdentityKey]                    INT            IDENTITY (0, 3) NOT NULL,
    [IdentifiantUtilisateurReceiver] NVARCHAR (450) NOT NULL,
    [IdentifiantUtilisateurRelated]  NVARCHAR (450) NULL,
    [_IdentityKeyEntityRelated]      NVARCHAR (MAX) NULL,
    [_TypeEntity]                    NVARCHAR (MAX) NULL,
    [NotificationName]               NVARCHAR (100) NOT NULL,
    [Seen]                           BIT            CONSTRAINT [DF__Notificati__Seen__34C8D9D1] DEFAULT ((0)) NOT NULL,
    [SeenAt]                         TIME (7)       NULL,
    [Message]                        TEXT           NOT NULL,
    CONSTRAINT [PK__Notifica__796424B8FAA2FE6F] PRIMARY KEY CLUSTERED ([IdentityKey] ASC),
    CONSTRAINT [ck_notification_entity_type] CHECK ([_TypeEntity]='Inscriptions' OR [_TypeEntity]='Demandes'),
    CONSTRAINT [ck_notification_type] CHECK ([NotificationName]='Communication' OR [NotificationName]='info' OR [NotificationName]='Success' OR [NotificationName]='Error'),
    CONSTRAINT [FK_Notifications_AspNetUsers] FOREIGN KEY ([IdentifiantUtilisateurReceiver]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Notifications_RelatedAspNetUser] FOREIGN KEY ([IdentifiantUtilisateurRelated]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

