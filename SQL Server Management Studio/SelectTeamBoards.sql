USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectTeamBoards] (@id TINYINT)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Board].TypeName, [TeamBoard].BoardSKU, [TeamBoard].TeamName, [TeamBoard].NumberOfBoards
	  FROM [TeamBoard]
	  LEFT JOIN [Board]
			ON [TeamBoard].[BoardSKU] = [Board].SKU 
	 WHERE DemandId = @id
	RETURN @@ROWCOUNT;
END