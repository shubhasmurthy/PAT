USE [PlatformAllocation]
GO

CREATE PROCEDURE PROC_CREATE_ROLE_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Role')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].Role (
		Id					TINYINT			NOT NULL ,
		Name				VARCHAR(30)		NOT NULL ,
		CONSTRAINT role_pk PRIMARY KEY (Id),
);


EXEC PROC_CREATE_ROLE_TABLE

SELECT * FROM [dbo].[Role];

INSERT INTO [dbo].Role (Id, Name) VALUES ('1', 'Administrator');
INSERT INTO [dbo].Role (Id, Name) VALUES ('2', 'TeamRep');
INSERT INTO [dbo].Role (Id, Name) VALUES ('3', 'TeamMgr');
