USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAdmin]
AS
BEGIN
DECLARE @RoleID INDICATOR;
	SET NOCOUNT ON;
	SELECT @RoleID = Id
	FROM [PlatformAllocation].[dbo].[Role]
	WHERE Name = 'Administrator'
	SELECT *
	  FROM [User]
	 WHERE RoleID=@RoleID and Active = 1
	RETURN @@ROWCOUNT;
END