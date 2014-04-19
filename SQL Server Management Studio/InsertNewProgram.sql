USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertNewProgram
	@ProgramName NAME
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Name FROM [dbo].Program WHERE Name = @ProgramName)
	INSERT
	INTO [Program](Name)
	VALUES (@ProgramName)
END
GO

