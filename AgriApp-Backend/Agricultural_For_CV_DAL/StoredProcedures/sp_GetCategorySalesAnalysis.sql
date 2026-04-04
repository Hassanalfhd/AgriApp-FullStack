USE [AgriculturalDB_For_CV]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCategorySalesAnalysis]    Script Date: 4/2/2026 11:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_GetCategorySalesAnalysis]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Name AS CategoryName,
        COUNT(od.Id) AS TotalOrdersCount,
        SUM(od.Quantity) AS TotalUnitsSold,
        SUM(od.Total) AS TotalRevenue
    FROM Categories c
    INNER JOIN Crops cr ON c.Id = cr.CategoryId
    INNER JOIN Products p ON cr.Id = p.CropTypeId
    INNER JOIN OrderDetails od ON p.Id = od.ProductId
    WHERE od.Status = 5 -- المبيعات الناجحة فقط
    GROUP BY c.Id, c.Name
    ORDER BY TotalRevenue DESC;
END;
