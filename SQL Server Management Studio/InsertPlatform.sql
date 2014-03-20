USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertPlatform 
	@name NAME,
	@user WWID
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Name FROM [dbo].Platform WHERE Name = @name)
	INSERT
	INTO [Platform](Name)
	VALUES (@name)
END
GO

