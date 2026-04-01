import { createAsyncThunk } from "@reduxjs/toolkit";
import { fetchSalesGrowthApi } from "../services/reportService";

export const getSalesGrowth = createAsyncThunk(
  "reports/getSalesGrowth",
  async (_, { rejectWithValue }) => {
    try {
      return await fetchSalesGrowthApi();
    } catch (error: any) {
      return rejectWithValue(
        error.response?.data || "حدث خطأ أثناء جلب بيانات النمو",
      );
    }
  },
);
