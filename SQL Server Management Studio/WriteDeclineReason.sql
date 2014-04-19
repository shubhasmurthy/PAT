USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE WriteDeclineReason 
	@reason VARCHAR(MAX),
	@demandId SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Demand]
	SET DeclineReason = @reason
	WHERE DemandId = @demandId
END
GO
