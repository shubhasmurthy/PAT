USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ListActiveDemandsPerUser] (@id VARCHAR(50)) 
AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @StateID INDICATOR
	SELECT @StateID = Id
	FROM [State]
	WHERE Name = 'Open'

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
			SELECT DISTINCT [Demand].DemandId AS DemandID, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate
			FROM [Demand]
			LEFT JOIN [TeamBoard]
			ON [Demand].DemandId = [TeamBoard].DemandId 
			WHERE [Demand].StateId = @StateID
			ORDER BY [Demand].DemandId
		End
	Else
		Begin
			SELECT DISTINCT [Demand].DemandId AS DemandId, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate
			FROM [Demand]
			LEFT JOIN [TeamBoard]
			ON [Demand].DemandId = [TeamBoard].DemandId 
			WHERE [TeamBoard].TeamName = (SELECT Name FROM Team WHERE ManagerId = @id OR RepresentativeId = @id)
			AND [Demand].StateId = @StateID
			ORDER BY [Demand].DemandId
		End
	RETURN @@ROWCOUNT;
END


	DECLARE @StateID INDICATOR
	SELECT @StateID = Id
	FROM [State]
	WHERE Name = 'Open'

	DECLARE @admnRole TINYINT
	SELECT @admnRole = Id
	FROM [Role]
	WHERE Name = 'Administrator'

	DECLARE @uRole TINYINT
	SELECT @uRole = RoleID
	FROM [User]
	WHERE ID = 'ssunda30'
	if(@admnRole = @uRole)
	Begin
			SELECT DISTINCT [Demand].DemandId AS DemandID, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate
			FROM [Demand]
			LEFT JOIN [TeamBoard]
			ON [Demand].DemandId = [TeamBoard].DemandId 
			WHERE [Demand].StateId = @StateID
			ORDER BY [Demand].DemandId
		End
	Else
		Begin
			SELECT DISTINCT [Demand].DemandId AS DemandId, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate
			FROM [Demand]
			LEFT JOIN [TeamBoard]
			ON [Demand].DemandId = [TeamBoard].DemandId 
			WHERE [TeamBoard].TeamName = (SELECT Name FROM Team WHERE ManagerId = 'ssunda30' OR RepresentativeId = 'ssunda30')
			AND [Demand].StateId = @StateID
			ORDER BY [Demand].DemandId
		End
	