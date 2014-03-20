USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectUserRoles] (@id IDSID) 
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