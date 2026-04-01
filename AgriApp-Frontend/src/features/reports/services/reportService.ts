import apiClient from "@/shared/services/apiClient";
import type { LowStockAlertDto } from "../types/LowStockAlertDto";
import type { SalesGrowthDto } from "../types/SalesGrowthDto";
import type { CategorySalesAnalysisDto } from "../types/CategorySalesAnalysisDto";
import type { FarmerFinancialSummaryDto } from "../types/FarmerMonthlySalesDtos";
import type { TopFarmerDto } from "../types/TopFarmerDto";

export const fetchLowStockAlertsApi = async (
  farmerId: number,
  threshold: number,
): Promise<LowStockAlertDto[]> => {
  console.log(farmerId);
  const response = await apiClient.get<LowStockAlertDto[]>(
    `v1/reports/stock-alerts`,
    {
      params: { farmerId, threshold },
    },
  );
  return response.data;
};

export const fetchSalesGrowthApi = async (): Promise<SalesGrowthDto[]> => {
  const response = await apiClient.get<SalesGrowthDto[]>(
    `v1/reports/admin/sales-growth`,
  );
  return response.data;
};

export const fetchCategoryAnalysisApi = async (): Promise<
  CategorySalesAnalysisDto[]
> => {
  const response = await apiClient.get<CategorySalesAnalysisDto[]>(
    `v1/Reports/admin/category-analysis`,
  );
  return response.data;
};

export const fetchFarmerMonthlySalesApi = async (
  farmerId: number,
  month: number,
  year: number,
): Promise<FarmerFinancialSummaryDto> => {
  const response = await apiClient.get<FarmerFinancialSummaryDto>(
    `v1/reports/farmer-sales/${farmerId}?year=${year}&month=${month}`,
  );
  return response.data;
};

export const fetchTopFarmersApi = async (
  count: number = 5,
): Promise<TopFarmerDto[]> => {
  const response = await apiClient.get<TopFarmerDto[]>(
    `v1/reports/admin/top-farmers`,
    {
      params: { count },
    },
  );
  return response.data;
};
