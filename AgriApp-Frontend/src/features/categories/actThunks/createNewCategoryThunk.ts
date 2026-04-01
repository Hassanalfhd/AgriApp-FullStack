import { apiCrudService } from "@/shared/services/apiCrudService";
import type { ICategoryToAddEdit } from "../types/categoriesTypes";
import type { RootState } from "@/app/store";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const createNewCategoryThunk = createAsyncThunk(
  "cateogries/createCategory",
  async (data: ICategoryToAddEdit, { rejectWithValue, getState }) => {
    try {
      const state = getState() as RootState;

      const token = state.auth.token;
      const userId = state.auth.user?.id;

      if (!token || !userId) return rejectWithValue("User not authenticated");

      const formData = new FormData();
      formData.append("Name", data.name);
      if (data.imageFile) {
        formData.append("ImageFile", data.imageFile); // File object من input
      }
      const category = await apiCrudService.create(
        "v1/categories/AddNewCategory",
        formData
      );

      return category;
    } catch (err: any) {
      console.error("Create Product Error:", err);
      return rejectWithValue(err.response?.data || "Error creating product");
    }
  }
);
