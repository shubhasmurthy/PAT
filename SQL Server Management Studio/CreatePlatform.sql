USE [PlatformAllocation]
GO

CREATE PROCEDURE PROC_CREATE_PLATFORM_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Platform')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].Platform (
		Name						NAME				NOT NULL,
		CONSTRAINT platform_pk PRIMARY KEY (Name) 	
);

SELECT * FROM [dbo].Platform;
DELETE FROM [dbo].Platform where Name != 'Platform1' OR Name != 'Platform2';

EXEC PROC_CREATE_PLATFORM_TABLE

INSERT INTO [dbo].Platform (Name)
 VALUES ('Platform1');

