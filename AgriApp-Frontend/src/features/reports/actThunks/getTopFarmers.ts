import { createAsyncThunk } from "@reduxjs/toolkit";
import { fetchTopFarmersApi } from "../services/reportService";

export const getTopFarmers = createAsyncThunk(
  "reports/getTopFarmers",
  async (count: number, { rejectWithValue }) => {
    try {
      return await fetchTopFarmersApi(count);
    } catch (error: any) {
      return rejectWithValue(
        error.response?.data || "Failed to fetch the top farmers list",
      );
    }
  },
);
