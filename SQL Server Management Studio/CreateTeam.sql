USE [PlatformAllocation]
GO

CREATE PROCEDURE PROC_CREATE_TEAM_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Team')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].Team (
		Name				NAME		NOT NULL,
		ManagerId			IDSID		NOT NULL,
		RepresentativeId	IDSID		NOT NULL,
		CONSTRAINT team_pk PRIMARY KEY (Name) 	
);


EXEC PROC_CREATE_TEAM_TABLE

INSERT INTO [dbo].Team (Name, ManagerId, RepresentativeId) VALUES ('Driver - Testing', 'ssundaMT', 'ssundaRT');
INSERT INTO [dbo].Team (Name, ManagerId, RepresentativeId) VALUES ('Driver - Development', 'ssundaMD', 'ssundaRD');
INSERT INTO [dbo].Team (Name, ManagerId, RepresentativeId) VALUES ('Driver - Validation', 'sundaDVA', 'sundaRVA');
INSERT INTO [dbo].Team (Name, ManagerId, RepresentativeId) VALUES ('Driver - Verification', 'sundaDVE', 'sundaRVE');

SELECT * FROM Team