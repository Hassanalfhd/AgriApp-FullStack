import { createAsyncThunk } from "@reduxjs/toolkit";
import type { ICrops } from "../types/cropsTypes";
import { apiCrudService } from "@/shared/services/apiCrudService";

export const getAllCropsThunk = createAsyncThunk<ICrops[]>(
  "crops/getAll",
  async (_, { rejectWithValue }) => {
    try {
      const categories = await apiCrudService.getAll<ICrops>("v1/crops/All");
      return categories;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);
