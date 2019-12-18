CREATE TABLE [dbo].[Comments]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PosterId] INT NOT NULL, 
    [ServiceId] INT NOT NULL, 
    [PostText] NTEXT NOT NULL , 
    [PostedDateTime] TIMESTAMP NOT NULL DEFAULT getdate()
)
