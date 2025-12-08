-- Week 15 - Stored Procedure for HSCMS
-- Returns products whose stock is at or below a given threshold.
-- This script should be committed to source control.

IF OBJECT_ID('dbo.GetLowStockProducts', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.GetLowStockProducts;
END
GO

CREATE PROCEDURE dbo.GetLowStockProducts
    @Threshold INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.ProductId,
        p.Name,
        p.Category,
        p.QuantityInStock,
        p.UnitPrice,
        p.SupplierId
    FROM dbo.Products AS p
    WHERE p.QuantityInStock <= @Threshold
    ORDER BY p.QuantityInStock ASC, p.Name;
END
GO
