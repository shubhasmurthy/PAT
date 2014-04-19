USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MonitorClosedDemands] 
AS
BEGIN
DECLARE @StateID INDICATOR;
DECLARE @OpenStateID INDICATOR;
DECLARE @SavedStateID INDICATOR;
DECLARE @DeclinedStateID INDICATOR;
	SET NOCOUNT ON;
	SELECT @StateID = Id
	FROM [State]
	WHERE Name = 'Closed'
	SELECT @OpenStateID = Id
	FROM [State]
	WHERE Name = 'Open'
	SELECT @SavedStateID = Id
	FROM [State]
	WHERE Name = 'Saved'
	SELECT @DeclinedStateID = Id
	FROM [State]
	WHERE Name = 'Declined'
	UPDATE [Demand]
	SET StateId=@StateID
	WHERE (Demand.StateId = @OpenStateID OR  Demand.StateId = @SavedStateID) AND (Demand.CloseDate < GETDATE())
END
