USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditDemand] 
	@demandID TINYINT,
	@name NAME,
	@pfName NAME,
	@prgName NAME,
	@openDate DATETIME,
	@closeDate DATETIME,
	@user WWID,
	@state VARCHAR(30)
AS
BEGIN
DECLARE @StateID INDICATOR;
	SET NOCOUNT ON;
	SELECT @StateID = Id
	FROM [State]
	WHERE Name = @state
	UPDATE [Demand]
	SET DemandName=@name, PlatformName=@pfName, ProgramName=@prgName, OpenDate=@openDate, CloseDate=@closeDate, StateId=@StateID
	WHERE [Demand].DemandId=@demandID
END