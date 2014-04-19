USE [PlatformAllocation]
GO

CREATE PROCEDURE PROC_CREATE_STATE_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('State')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].State (
		Id					INDICATOR	NOT NULL,
		Name				NAME		NOT NULL,
		CONSTRAINT state_pk PRIMARY KEY (Id) 	
);


EXEC PROC_CREATE_STATE_TABLE


INSERT INTO [dbo].State (Id, Name) VALUES ('1', 'Open');
INSERT INTO [dbo].State (Id, Name) VALUES ('2', 'Saved');
INSERT INTO [dbo].State (Id, Name) VALUES ('3', 'Approved');
INSERT INTO [dbo].State (Id, Name) VALUES ('4', 'Closed');
INSERT INTO [dbo].State (Id, Name) VALUES ('6', 'Declined');

DELETE FROM [dbo].State WHERE Id='0';

SELECT * FROM [dbo].State