USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ListDemandsPerUser] (@id IDSID) 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [User].RoleID, [Role].Name
	  FROM [User]
	  LEFT JOIN [Role]
	    ON [User].RoleID = [Role].Id
	 WHERE [User].ID = @id
	 ORDER BY [User].ID
	RETURN @@ROWCOUNT;
END

SELECT [Demand].DemandId, [Demand].DemandName, [Demand].PlatformName, [Demand].ProgramName, [Demand].CloseDate
FROM [Demand]
LEFT JOIN [TeamBoard]
ON [Demand].DemandId = [TeamBoard].DemandId 
WHERE [TeamBoard].TeamName = 'Driver1 - Development'
ORDER BY [Demand].DemandId
