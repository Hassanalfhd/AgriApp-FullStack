import { apiCrudService } from "@/shared/services/apiCrudService";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { ICrops } from "../types/cropsTypes";

export const getCropByIdThunk = createAsyncThunk(
  "crop/getCropById",
  async (id: number | string, { rejectWithValue }) => {
    try {
      const crop = await apiCrudService.getById<ICrops>("v1/crops", id);
      console.log("cropsdfl", crop);
      return crop;
    } catch (err: any) {
      console.error("Get Crop Error:", err);
      return rejectWithValue(err.response?.data || "Error Getting Crop");
    }
  }
);
