USE [PlatformAllocation]
GO

CREATE PROCEDURE PROC_CREATE_BOARD_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Board')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].Board (
		SKU							SKU				NOT NULL,
		TypeName					NAME,
		CONSTRAINT board_pk PRIMARY KEY (SKU),
);

EXEC PROC_CREATE_BOARD_TABLE

SELECT * FROM [dbo].Board;

DELETE FROM [dbo].Board
WHERE TypeName ='BoardType3';

INSERT INTO [dbo].Board (SKU, TypeName) VALUES ('BT2-SKU1', 'BoardType2');
INSERT INTO [dbo].Board (SKU, TypeName) VALUES ('BT1-SKU1', 'BoardType1');
INSERT INTO [dbo].Board (SKU, TypeName) VALUES ('BT2-SKU2', 'BoardType2');
INSERT INTO [dbo].Board (SKU, TypeName) VALUES ('BT1-SKU2', 'BoardType1');

IF NOT EXISTS (SELECT SKU FROM [dbo].Board WHERE TypeName = 'BoardType3' AND SKU ='BT3-SKU2')
    INSERT INTO [dbo].Board (SKU, TypeName, CreatedUser, ModifiedDate, ModifiedUser) 
	VALUES ('BT2-SKU2', 'BoardType3', '11111111', GETDATE(), '11111111');

