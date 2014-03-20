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
	@numberOfBoards TINYINT,
	@user WWID
AS
BEGIN
	SET NOCOUNT ON;
	INSERT
	INTO [TeamBoard](BoardSKU, DemandId, TeamName, NumberOfBoards)
	VALUES (@sku, @demandID, @team, @numberOfBoards)
END
GO
