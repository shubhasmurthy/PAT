USE [PlatformAllocation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertNewBoard 
	@sku SKU,
	@typeName NAME,
	@user WWID
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT SKU FROM [dbo].Board WHERE TypeName = @typeName AND SKU = @sku)
	INSERT
	INTO [Board](SKU, TypeName)
	VALUES (@sku, @typeName)
END
GO

