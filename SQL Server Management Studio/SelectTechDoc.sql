USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectTechDoc] (@id TINYINT)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TechnicalDocumentation.TDocName, TechnicalDocumentation.Url
	  FROM [TechnicalDocumentation]
	 WHERE [TechnicalDocumentation].DemandId = @id
	RETURN @@ROWCOUNT;
END