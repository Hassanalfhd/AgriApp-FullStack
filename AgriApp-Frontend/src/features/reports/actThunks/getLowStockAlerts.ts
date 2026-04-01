import { createAsyncThunk } from "@reduxjs/toolkit";
import { fetchLowStockAlertsApi } from "../services/reportService";
import type { RootState } from "@/app/store";

export const getLowStockAlerts = createAsyncThunk(
  "reports/getLowStockAlerts",
  async (threshold: number, { rejectWithValue, getState }) => {
    try {

      const state = getState() as RootState;
      const farmerId = state.auth.user?.id;
      console.log(farmerId);
      return await fetchLowStockAlertsApi(farmerId, threshold);
    } catch (error: any) {
      return rejectWithValue(error.response?.data || "Failed to fetch alerts");
    }
  },
);
