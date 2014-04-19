USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateDemandStatus] 
	@demandID TINYINT,
	@state VARCHAR(30)
AS
BEGIN
DECLARE @StateID INDICATOR;
	SET NOCOUNT ON;
	SELECT @StateID = Id
	FROM [State]
	WHERE Name = @state
	UPDATE [Demand]
	SET StateId=@StateID
	WHERE [Demand].DemandId=@demandID
END
