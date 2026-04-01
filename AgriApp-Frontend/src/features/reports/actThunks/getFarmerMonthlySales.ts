import { createAsyncThunk } from "@reduxjs/toolkit";
import { fetchFarmerMonthlySalesApi } from "../services/reportService";

export const getFarmerMonthlySales = createAsyncThunk(
  "reports/getFarmerMonthlySales",
  async (
    {
      farmerId,
      month,
      year,
    }: { farmerId: number; month: number; year: number },
    { rejectWithValue },
  ) => {
    try {
      return await fetchFarmerMonthlySalesApi(farmerId, month, year);
    } catch (error: any) {
      return rejectWithValue(
        error.response?.data ||
          "Failed to load the report, or no report is available for this period.",
      );
    }
  },
);
