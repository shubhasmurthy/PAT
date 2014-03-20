USE PlatformAllocation
GO

CREATE PROCEDURE dbo.PROC_CREATE_PROGRAM_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Program')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].Program (
		Name			NAME				NOT NULL,
		CONSTRAINT program_pk PRIMARY KEY (Name),
);

EXEC PROC_CREATE_PROGRAM_TABLE

INSERT INTO [dbo].[Program] (Name)  VALUES ('Program1');

DELETE FROM [PlatformAllocation].[dbo].Program;

SELECT * FROM [dbo].Program