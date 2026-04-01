import { apiCrudService } from "@/shared/services/apiCrudService";
import type { RootState } from "@/app/store";
import type { ICrops } from "../types/cropsTypes";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const addCropThunk = createAsyncThunk(
  "crops/addCrops",
  async (data: ICrops, { rejectWithValue, getState }) => {
    try {
      const state = getState() as RootState;

      const token = state.auth.token;
      const userId = state.auth.user?.id;

      if (!token || !userId) return rejectWithValue("User not authenticated");

      data.ownerId = userId;
      const formData = new FormData();
      formData.append("Name", data.name);
      formData.append("OwnerId", data.ownerId.toString());
      formData.append("CategoryId", data.categoryId.toString());

      if (data.imageFile) {
        formData.append("imagePath", data.imageFile); // File object من input
      }

      const category = await apiCrudService.create(
        "v1/crops/AddNewCrop",
        formData
      );

      return category;
    } catch (err: any) {
      console.error("Create Crop Error:", err);
      return rejectWithValue(err.response?.data || "Error creating Crop");
    }
  }
);
