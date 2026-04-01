import { createAsyncThunk } from "@reduxjs/toolkit";
import { fetchCategoryAnalysisApi } from "../services/reportService";

export const getCategoryAnalysis = createAsyncThunk(
  "reports/getCategoryAnalysis",
  async (_, { rejectWithValue }) => {
    try {
      return await fetchCategoryAnalysisApi();
    } catch (error: any) {
      return rejectWithValue(error.response?.data || "فشل تحميل تحليل الأقسام");
    }
  },
);
