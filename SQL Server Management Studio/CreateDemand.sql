USE [PlatformAllocation]
GO

CREATE PROCEDURE PROC_CREATE_DEMAND_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Demand')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].Demand (
		DemandId							SMALLINT IDENTITY(1,1)	NOT NULL, 
		DemandName							NAME					NOT NULL,
		PlatformName						NAME					NOT NULL,
		ProgramName							NAME					NOT NULL,
		OpenDate							DATETIME				NOT NULL DEFAULT GETDATE(),
		CloseDate							DATETIME				NOT NULL,
		StateId								INDICATOR				NOT NULL,
		CONSTRAINT demand_pk PRIMARY KEY (DemandId),
		CONSTRAINT fk_demand_platform FOREIGN KEY (PlatformName) REFERENCES Platform(Name), 
		CONSTRAINT fk_demand_program FOREIGN KEY (ProgramName) REFERENCES Program(Name),
		CONSTRAINT fk_demand_state FOREIGN KEY (StateId) REFERENCES [State](Id),
);


EXEC PROC_CREATE_DEMAND_TABLE


UPDATE [dbo].Demand SET StateId='1' WHERE StateId='0';

Select * from [PlatformAllocation].[dbo].[Demand];

DELETE FROM [PlatformAllocation].[dbo].[Demand];

DBCC CHECKIDENT('[PlatformAllocation].[dbo].[Demand]', RESEED, 0)