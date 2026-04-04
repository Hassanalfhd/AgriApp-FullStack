
USE [AgriculturalDB_For_CV]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLowStockAlerts]    Script Date: 4/2/2026 11:18:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER PROCEDURE [dbo].[sp_GetLowStockAlerts]
    @FarmerId INT = NULL,
    @Threshold INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id AS ProductId,
        p.Name AS ProductName,
        p.QuantityInStock AS CurrentStock,
        c.Name AS CategoryName, -- نأتي به من جدول الأقسام عبر المحصول
        u.fullName AS FarmerName
    FROM Products p
    INNER JOIN Crops cr ON p.CropTypeId = cr.Id        -- الربط الأول: المنتج بالمحصول
    INNER JOIN Categories c ON cr.CategoryId = c.Id -- الربط الثاني: المحصول بالقسم
    INNER JOIN Users u ON p.CreatedBy = u.Id        -- الربط الثالث: المنتج بالمزارع
    WHERE p.QuantityInStock <= @Threshold
      AND (@FarmerId IS NULL OR p.CreatedBy= @FarmerId)
    ORDER BY p.QuantityInStock ASC;
END;

