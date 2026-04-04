

USE [AgriculturalDB_For_CV]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSalesGrowthReport]    Script Date: 4/2/2026 11:18:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER Procedure [dbo].[sp_GetSalesGrowthReport]
As
Begin

Set NOCOUNT ON;

WITH MonthlySales As (
	Select 
		YEAR(o.CreatedAt) As SalesYear,
		Month(o.CreatedAt) As SalesMonth,
		Sum(od.Total) As CurrentMonthRevenue
	From Orders o
	Join OrderDetails od On o.Id = od.OrderId
	Where od.Status = 5 --- completed
	Group By YEAR(o.CreatedAt), MONTH(o.CreatedAt)
),
	GrowthCalculation As (
		Select 
			SalesYear,
            SalesMonth,
            CurrentMonthRevenue,
			LAG(CurrentMonthRevenue)Over (Order By SalesYear, SalesMonth)AS PreviousMonthRevenue
		From MonthlySales
)
	SELECT 
			SalesYear,
			SalesMonth,
			CurrentMonthRevenue,
			ISNULL(PreviousMonthRevenue, 0) AS PreviousMonthRevenue,
			-- حساب نسبة النمو (Growth Percentage)
			CASE 
				WHEN PreviousMonthRevenue IS NULL OR PreviousMonthRevenue = 0 THEN 100
				ELSE ((CurrentMonthRevenue - PreviousMonthRevenue) / PreviousMonthRevenue) * 100 
			END AS GrowthPercentage
		FROM GrowthCalculation
		ORDER BY SalesYear DESC, SalesMonth DESC;


End;

