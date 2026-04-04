
USE [AgriculturalDB_For_CV]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTopFarmersReport]    Script Date: 4/2/2026 11:18:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Or ALTER PROCEDURE [dbo].[sp_GetTopFarmersReport]
    @TopCount INT = 5,
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopCount)
        u.Id AS FarmerId,
        u.fullName AS FarmerName,
        COUNT(DISTINCT od.OrderId) AS TotalOrdersHandled,
        SUM(od.Quantity) AS TotalProductsSold,
        SUM(od.Total) AS TotalRevenue
    FROM Users u
    INNER JOIN OrderDetails od ON u.Id = od.FarmerId
    INNER JOIN Orders o ON od.OrderId = o.Id
    WHERE u.UserType = 2 -- Farmer
      AND o.CreatedAt >= @StartDate 
      AND o.CreatedAt <= @EndDate
      AND od.Status = 5
    GROUP BY u.Id, u.fullName
    ORDER BY TotalRevenue DESC;
END;
