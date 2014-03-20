USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertDemand] 
	@demandID TINYINT OUTPUT,
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
	INSERT
	INTO [Demand](DemandName, PlatformName, ProgramName, OpenDate, CloseDate, StateId)
	VALUES (@name, @pfName, @prgName, @openDate, @closeDate, @StateID)
	SET @demandID = SCOPE_IDENTITY();
END