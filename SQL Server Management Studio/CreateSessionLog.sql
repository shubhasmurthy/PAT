USE PlatformAllocation
GO

CREATE PROCEDURE dbo.PROC_CREATE_SESSIONLOG_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('SessionLog')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].SessionLog (
		EntryType					VARCHAR(12)					NOT NULL,
		UserName					VARCHAR(30),
		ServerName					VARCHAR(20),
		[Platform]					VARCHAR(30),
		Browser						VARCHAR(30),
		StackTraceID				int,
		[Message]					VARCHAR(250),
		AssemblyVersion				VARCHAR(13),
		TimeoutMinutes				VARCHAR(3),
		SessionLength				int,
		SessionID					char(30),
		SessionStart				DATETIME,		
		CreatedDate					DATETIME			NOT NULL DEFAULT GETDATE(),
		CreatedUser					WWID				NOT NULL,
);
GO

EXEC PROC_CREATE_SESSIONLOG_TABLE