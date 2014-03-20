USE [PlatformAllocation_pre]
GO

CREATE PROCEDURE PROC_CREATE_USER_ROLE_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('UserRole')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].UserRole (
		UserId				IDSID		NOT NULL,
		RoleId				NAME		NOT NULL,
		CreatedDate			DATETIME	NOT NULL DEFAULT GETDATE(),
		CreatedUser			WWID		NOT NULL,
		ModifiedDate		DATETIME,
		ModifiedUser		WWID,
);


EXEC PROC_CREATE_USER_ROLE_TABLE

SELECT * FROM [dbo].UserRole;

INSERT INTO [dbo].UserRole (UserId, RoleId, CreatedUser, ModifiedDate, ModifiedUser) VALUES ('shubhasu', '1', '11111111', GETDATE(), '11111111');