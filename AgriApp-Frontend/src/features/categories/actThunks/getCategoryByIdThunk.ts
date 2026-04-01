import { apiCrudService } from "@/shared/services/apiCrudService";
import type { ICategory } from "../types/categoriesTypes";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const getCategoryByIdThunk = createAsyncThunk(
  "cateogries/getCategoryById",
  async (id: number | string, { rejectWithValue }) => {
    try {
      const category = await apiCrudService.getById<ICategory>(
        "v1/categories",
        id
      );

      return category;
    } catch (err: any) {
      console.error("Found Category  Error:", err);
      return rejectWithValue(err.response?.data || "Error Found Category");
    }
  }
);
