USE [PlatformAllocation_pre]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertDemand 
	@demandID TINYINT OUTPUT,
	@name NAME,
	@pfName NAME,
	@prgName NAME,
	@openDate DATETIME,
	@closeDate DATETIME,
	@user WWID,
	@state VARCHAR(30)
AS
BEGIN
DECLARE @StateID INDICATOR;
	SET NOCOUNT ON;
	SELECT @StateID = Id
	FROM [State]
	WHERE Name = @state
	INSERT
	INTO [Demand](DemandName, PlatformName, ProgramName, OpenDate, CloseDate, StateId, CreatedUser, ModifiedDate, ModifiedUser)
	VALUES (@name, @pfName, @prgName, @openDate, @closeDate, @StateID, @user, GETDATE(), @user)
	SET @demandID = SCOPE_IDENTITY();
END
GO
