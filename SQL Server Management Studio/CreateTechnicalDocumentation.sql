USE PlatformAllocation
GO

CREATE PROCEDURE dbo.PROC_CREATE_TECHNICALDOCUMENTATION_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TechnicalDocumentation')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].TechnicalDocumentation (
		DemandId							SMALLINT			NOT NULL,
		TDocName							NAME				NOT NULL,
		Url									URL					NOT NULL,
		CONSTRAINT fk_technicaldocumentation_demand FOREIGN KEY (DemandId) REFERENCES Demand(DemandId) 	
);
GO

EXEC PROC_CREATE_TECHNICALDOCUMENTATION_TABLE

Select * from [PlatformAllocation].[dbo].[TechnicalDocumentation];

DELETE FROM [PlatformAllocation].[dbo].[TechnicalDocumentation];