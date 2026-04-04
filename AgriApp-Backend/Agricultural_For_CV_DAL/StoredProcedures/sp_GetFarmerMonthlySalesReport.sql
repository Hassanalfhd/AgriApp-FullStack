
USE [AgriculturalDB_For_CV]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetFarmerMonthlySalesReport]    Script Date: 4/2/2026 11:18:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_GetFarmerMonthlySalesReport]
    @FarmerId INT,
    @StartDate DATETIME, -- نرسل مثلاً '2026-03-01'
    @EndDate DATETIME    -- نرسل مثلا'2026-03-31 23:59:59'
AS
BEGIN
    SET NOCOUNT ON; -- لتحسين الأداء بتقليل رسائل الشبكة

    SELECT 
        p.Name AS ProductName,
        SUM(od.Quantity) AS TotalQuantitySold,
        SUM(od.Total) AS TotalRevenue,
        COUNT(DISTINCT od.OrderId) AS NumberOfOrders
    FROM OrderDetails od WITH (INDEX(IX_OrderDetails_Farmer_Reporting)) -- إجبار استخدام الفهرس المطور
    JOIN Products p ON od.ProductId = p.Id
    JOIN Orders o ON od.OrderId = o.Id
    WHERE od.FarmerId = @FarmerId 
      AND o.CreatedAt >= @StartDate 
      AND o.CreatedAt <= @EndDate
      AND od.Status = 5 -- Completed
    GROUP BY p.Name;
END;

