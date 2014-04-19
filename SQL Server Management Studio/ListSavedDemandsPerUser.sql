USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ListSavedDemandsPerUser] (@id VARCHAR(50)) 
AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @SavedStateID INDICATOR
	DECLARE @DeclinedStateID INDICATOR
	SELECT @SavedStateID = Id
	FROM [State]
	WHERE Name = 'Saved'

	DECLARE @admnRole TINYINT
	SELECT @admnRole = Id
	FROM [Role]
	WHERE Name = 'Administrator'

	DECLARE @uRole TINYINT
	SELECT @uRole = RoleID
	FROM [User]
	WHERE ID = @id
	if(@admnRole = @uRole)
	Begin
			SELECT DISTINCT [Demand].DemandId AS DemandID, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate, [TeamBoard].TeamName
			FROM [Demand]
			LEFT JOIN [TeamBoard]
			ON [Demand].DemandId = [TeamBoard].DemandId 
			WHERE [Demand].StateId = @SavedStateID 
			ORDER BY [Demand].DemandId
		End
	Else
		Begin
			SELECT DISTINCT [Demand].DemandId AS DemandId, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate, [TeamBoard].TeamName
			FROM [Demand]
			LEFT JOIN [TeamBoard]
			ON [Demand].DemandId = [TeamBoard].DemandId 
			WHERE [TeamBoard].TeamName = (SELECT Name FROM Team WHERE ManagerId = @id OR RepresentativeId = @id)
			AND [Demand].StateId = @SavedStateID 
			ORDER BY [Demand].DemandId
		End
	RETURN @@ROWCOUNT;
END
