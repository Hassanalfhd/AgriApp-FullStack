import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { LowStockAlertDto } from "../types/LowStockAlertDto";
import { getLowStockAlerts } from "../actThunks/getLowStockAlerts";
import type { SalesGrowthDto } from "../types/SalesGrowthDto";
import { getSalesGrowth } from "../actThunks/getSalesGrowth";
import type { CategorySalesAnalysisDto } from "../types/CategorySalesAnalysisDto";
import { getCategoryAnalysis } from "../actThunks/getCategoryAnalysis";
import type { FarmerFinancialSummaryDto } from "../types/FarmerMonthlySalesDtos";
import { getFarmerMonthlySales } from "../actThunks/getFarmerMonthlySales";
import type { TopFarmerDto } from "../types/TopFarmerDto";
import { getTopFarmers } from "../actThunks/getTopFarmers";

interface ReportState {
  lowStockAlerts: LowStockAlertDto[];
  loading: boolean;
  farmerFinancialSummary: FarmerFinancialSummaryDto | null;
  salesGrowth: SalesGrowthDto[];
  categoryAnalysis: CategorySalesAnalysisDto[];
  topFarmers: TopFarmerDto[];
  error: string | null;
}

const initialState: ReportState = {
  lowStockAlerts: [],
  salesGrowth: [],
  categoryAnalysis: [],
  farmerFinancialSummary: null,
  topFarmers: [],
  loading: false,
  error: null,
};

const reportSlice = createSlice({
  name: "reports",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(getLowStockAlerts.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(
        getLowStockAlerts.fulfilled,
        (state, action: PayloadAction<LowStockAlertDto[]>) => {
          state.loading = false;
          state.lowStockAlerts = action.payload;
        },
      )
      .addCase(getLowStockAlerts.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(getSalesGrowth.pending, (state) => {
        state.loading = true;
      })
      .addCase(
        getSalesGrowth.fulfilled,
        (state, action: PayloadAction<SalesGrowthDto[]>) => {
          state.loading = false;
          state.salesGrowth = action.payload;
        },
      )
      .addCase(getSalesGrowth.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(getCategoryAnalysis.pending, (state) => {
        state.loading = true;
      })
      .addCase(
        getCategoryAnalysis.fulfilled,
        (state, action: PayloadAction<CategorySalesAnalysisDto[]>) => {
          state.loading = false;
          state.categoryAnalysis = action.payload;
          console.log(action.payload);
        },
      )
      .addCase(getCategoryAnalysis.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(getFarmerMonthlySales.pending, (state) => {
        state.loading = true;
      })
      .addCase(
        getFarmerMonthlySales.fulfilled,
        (state, action: PayloadAction<FarmerFinancialSummaryDto>) => {
          state.loading = false;
          state.farmerFinancialSummary = action.payload;
        },
      )
      .addCase(getFarmerMonthlySales.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(getTopFarmers.pending, (state) => {
        state.loading = true;
      })
      .addCase(
        getTopFarmers.fulfilled,
        (state, action: PayloadAction<TopFarmerDto[]>) => {
          state.loading = false;
          state.topFarmers = action.payload;
        },
      )
      .addCase(getTopFarmers.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default reportSlice.reducer;
