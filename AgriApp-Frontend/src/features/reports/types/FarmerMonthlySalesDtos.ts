export interface FarmerMonthlySalesDto {
  productName: string;
  totalQuantitySold: number;
  totalRevenue: number;
  numberOfOrders: number;
}

export interface FarmerFinancialSummaryDto {
  monthlyData: FarmerMonthlySalesDto[];
  grossRevenue: number; // إجمالي الإيرادات
  netRevenue: number; // صافي الأرباح (بعد الخصم)
  platformCommission: number; // عمولة المنصة
  reportDate: string;
}
