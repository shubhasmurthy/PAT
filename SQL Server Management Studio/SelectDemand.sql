USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectDemand] (@id TINYINT)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DemandId, DemandName, PlatformName, ProgramName, OpenDate, CloseDate, [State].Name, DeclineReason
	  FROM [Demand]
	  LEFT JOIN [State]
			ON [Demand].[StateId] = [State].Id 
	 WHERE DemandId = @id
	RETURN @@ROWCOUNT;
END