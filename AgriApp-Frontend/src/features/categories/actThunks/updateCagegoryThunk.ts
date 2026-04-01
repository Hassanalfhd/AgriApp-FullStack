import { apiCrudService } from "@/shared/services/apiCrudService";
import type { ICategoryToAddEdit } from "../types/categoriesTypes";
import type { RootState } from "@/app/store";
import { createAsyncThunk } from "@reduxjs/toolkit";
interface UpdatePayload {
  id: number | string;
  data: ICategoryToAddEdit;
}

export const updateCagegoryThunk = createAsyncThunk(
  "cateogries/updateCategory",
  async ({ id, data }: UpdatePayload, { rejectWithValue, getState }) => {
    try {
      console.log("id2", id);
      const state = getState() as RootState;

      const token = state.auth.token;
      const userId = state.auth.user?.id;

      if (!token || !userId) return rejectWithValue("User not authenticated");
      const categoryId = typeof id === "string" ? parseInt(id) : id;

      const formData = new FormData();
      formData.append("id", categoryId);
      formData.append("Name", data.name);

      if (data.imageFile) {
        formData.append("ImageFile", data.imageFile); // File object من input
      }

      const category = await apiCrudService.update(
        "v1/categories/UpdateCategory",
        formData,
      );

      return category;
    } catch (err: any) {
      console.error("Create Product Error:", err);
      return rejectWithValue(err.response?.data || "Error creating product");
    }
  },
);
