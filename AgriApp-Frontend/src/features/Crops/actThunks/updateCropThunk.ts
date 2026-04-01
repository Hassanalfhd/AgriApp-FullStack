import { apiCrudService } from "@/shared/services/apiCrudService";
import type { RootState } from "@/app/store";
import type { ICrops } from "../types/cropsTypes";
import { createAsyncThunk } from "@reduxjs/toolkit";
interface UpdatePayload {
  id: number | string;
  data: ICrops;
}

export const updateCropThunk = createAsyncThunk(
  "crops/updateCrops",
  async ({ id, data }: UpdatePayload, { rejectWithValue, getState }) => {
    try {
      console.log("id2", id);
      const state = getState() as RootState;

      const token = state.auth.token;
      const userId = state.auth.user?.id;

      if (!token || !userId) return rejectWithValue("User not authenticated");
      const cropId = typeof id === "string" ? parseInt(id) : id;

      data.ownerId = userId;
      const formData = new FormData();
      formData.append("id", cropId);
      formData.append("Name", data.name);
      formData.append("OwnerId", data.ownerId.toString());
      formData.append("CategoryId", data.categoryId.toString());

      if (data.imageFile) {
        formData.append("ImagePath", data.imageFile); // File object من input
      }

      const category = await apiCrudService.update(
        "v1/crops/UpdateCrop",
        formData
      );

      return category;
    } catch (err: any) {
      console.error("Update Crop Error:", err);
      return rejectWithValue(err.response?.data || "Error Updating Crop");
    }
  }
);
