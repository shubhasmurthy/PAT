USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE ClearTeamBoard 
	@demandID TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE
	FROM [TeamBoard]
	WHERE [TeamBoard].DemandId = @demandID
END
GO
