USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE ClearTechDoc 
	@demandID TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE
	FROM [TechnicalDocumentation]
	WHERE [TechnicalDocumentation].DemandId = @demandID
END
GO
