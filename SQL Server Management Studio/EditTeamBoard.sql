USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE EditTeamBoard 
	@sku VARCHAR(10),
	@team VARCHAR(30),
	@demandID TINYINT,
	@numberOfBoards TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [TeamBoard]
	SET [TeamBoard].NumberOfBoards = @numberOfBoards
	WHERE [TeamBoard].DemandId = @demandID AND [TeamBoard].TeamName = @team AND [TeamBoard].BoardSKU = @sku
END
GO
