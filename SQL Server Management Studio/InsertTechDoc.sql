USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertTechDoc 
	@name NAME,
	@link URL,
	@demandID TINYINT,
	@user WWID
AS
BEGIN
	SET NOCOUNT ON;
	INSERT
	INTO [TechnicalDocumentation](DemandId, TDocName, Url)
	VALUES (@demandID, @name, @link)
END
GO
