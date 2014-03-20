USE PlatformAllocation
GO

CREATE PROCEDURE dbo.PROC_CREATE_TEAMBOARD_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TeamBoard')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].TeamBoard (
		BoardSKU					SKU					NOT NULL,
		DemandId					SMALLINT			NOT NULL,
		TeamName					NAME				NOT NULL,
		NumberOfBoards				TINYINT				NOT NULL,
		CONSTRAINT teamboard_pk PRIMARY KEY (BoardSKU, DemandId, TeamName),
		CONSTRAINT fk_teamboard_board FOREIGN KEY (BoardSKU) REFERENCES Board(SKU),
		CONSTRAINT fk_teamboard_demand FOREIGN KEY (DemandId) REFERENCES Demand(DemandId),
		CONSTRAINT fk_teamboard_team FOREIGN KEY (TeamName) REFERENCES Team(Name),
);
GO

EXEC PROC_CREATE_TEAMBOARD_TABLE


Select * from [PlatformAllocation].[dbo].[TeamBoard];

DELETE FROM [PlatformAllocation].[dbo].[TeamBoard] where DemandId = 7;