USE PlatformAllocation
GO

CREATE PROCEDURE dbo.PROC_CREATE_USER_TABLE
AS
SET NOCOUNT ON

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id('User')
AND OBJECTPROPERTY(id, 'IsUserTable') = 1)

CREATE TABLE [dbo].[User] (
		ID				IDSID			NOT NULL ,
		[Password]		VARCHAR(20)		NOT NULL ,
		RoleID			TINYINT			NOT NULL ,
        FirstName		VARCHAR(20)		NOT NULL , 
        LastName		VARCHAR(20)		NOT NULL , 
        eAddress		VARCHAR(60)		NOT NULL ,
        WWID			WWID			NOT NULL ,
        Active			BIT				NOT NULL	DEFAULT 1,
		CONSTRAINT user_pk PRIMARY KEY (ID),
		CONSTRAINT fk_user_role FOREIGN KEY (RoleID) REFERENCES [Role](Id),
);
GO

EXEC PROC_CREATE_USER_TABLE

INSERT INTO [dbo].[User] (ID, [Password], [RoleID], FirstName, LastName, eAddress, WWID)
 VALUES ('ssunda30', 'password', 1, 'Shubha', 'Murthy', 'shubhasmurthy@asu.com', '54348765');

 INSERT INTO [dbo].[User] (ID, [Password], [RoleID], FirstName, LastName, eAddress, WWID)
 VALUES ('ssundaMD', 'password', 3, 'Shubha', 'Murthy', 'shubhasmurthy@asu.com', '63736500');

  INSERT INTO [dbo].[User] (ID, [Password], [RoleID], FirstName, LastName, eAddress, WWID)
 VALUES ('ssundaMT', 'password', 3, 'Shubha', 'Murthy', 'shubhasmurthy@asu.com', '77185629');

  INSERT INTO [dbo].[User] (ID, [Password], [RoleID], FirstName, LastName, eAddress, WWID)
 VALUES ('ssundaTD', 'password', 2, 'Shubha', 'Murthy', 'shubhasmurthy@asu.com', '69968220');

   INSERT INTO [dbo].[User] (ID, [Password], [RoleID], FirstName, LastName, eAddress, WWID)
 VALUES ('ssundaRT', 'password', 2, 'Shubha', 'Murthy', 'shubhasmurthy@asu.com', '18185637');

SELECT * FROM [User]

DELETE FROM [User]

Select COUNT(ID) from [dbo].[User]
	where [ID] = 'ssunda30' and [Password] = 'password'