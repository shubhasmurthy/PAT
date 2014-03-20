USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertLogs] 
	@type VARCHAR(12),
	@userName VARCHAR(30),
	@serverName VARCHAR(20),
	@platform VARCHAR(30),
	@browser VARCHAR(30),
	@stackTrace VARCHAR(2000),
	@message VARCHAR(250),
	@assemblyVersion VARCHAR(13),
	@timeoutMinutes VARCHAR(3),
	@sessionLength INT,
	@sessionID CHAR(32),
	@sessionStart DATETIME,
	@entryID VARCHAR(8)
AS
BEGIN
DECLARE @StackTraceID INT;
	SET NOCOUNT ON;
	
	IF LEN(@stackTrace) > 0
	BEGIN
		INSERT
		  INTO StackTraceLog (StackTrace, CreatedUser, CreatedDate)
		VALUES (@stackTrace, @entryID, GETDATE())
		SET @StackTraceID = SCOPE_IDENTITY()
		INSERT
		  INTO SessionLog (EntryType, UserName, ServerName, Platform, Browser, StackTraceID, Message, AssemblyVersion, TimeoutMinutes, SessionLength, SessionID, SessionStart, CreatedUser, CreatedDate)
		VALUES (@type, @userName, @serverName, @platform, @browser, @StackTraceID, @message, @assemblyVersion, @timeoutMinutes, @sessionLength, @sessionID, @sessionStart, @entryID, GETDATE())
		RETURN @@ROWCOUNT;
	END
	ELSE
		INSERT
		  INTO SessionLog (EntryType, UserName, ServerName, Platform, Browser, Message, AssemblyVersion, TimeoutMinutes, SessionLength, SessionID, SessionStart, CreatedUser, CreatedDate)
		VALUES (@type, @userName, @serverName, @platform, @browser, @message, @assemblyVersion, @timeoutMinutes, @sessionLength, @sessionID, @sessionStart, @entryID, GETDATE())
		RETURN @@ROWCOUNT;
END