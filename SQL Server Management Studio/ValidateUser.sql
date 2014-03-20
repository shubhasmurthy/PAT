USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE ValidateUser
@UserName IDSID,
@Password NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	Declare @Count int
	Select @Count = COUNT(ID) from [dbo].[User]
	where [ID] = @UserName and [Password] = @Password
	if(@Count = 1)
		Begin
			Select 1 as ReturnCode
		End
	Else
		Begin
			Select -1 as ReturnCode
		End

END

