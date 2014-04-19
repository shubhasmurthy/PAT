USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectTeam] 
	@mgrID IDSID OUTPUT,
	@repID IDSID OUTPUT,
	@mgrEmailID VARCHAR(60) OUTPUT,
	@repEmailID VARCHAR(60) OUTPUT,
	@name NAME
AS
BEGIN
	SET NOCOUNT ON;
	SELECT @mgrID=[PlatformAllocation].[dbo].[Team].ManagerId, @repID=[PlatformAllocation].[dbo].[Team].RepresentativeId
	  FROM [PlatformAllocation].[dbo].[Team]
	WHERE Name = @name
	SELECT @mgrEmailID=[PlatformAllocation].[dbo].[User].eAddress
	  FROM [PlatformAllocation].[dbo].[User]
	WHERE ID = @mgrID
	SELECT @repEmailID=[PlatformAllocation].[dbo].[User].eAddress
	  FROM [PlatformAllocation].[dbo].[User]
	WHERE ID = @repID
END