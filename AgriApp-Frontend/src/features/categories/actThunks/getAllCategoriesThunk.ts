import { createAsyncThunk } from "@reduxjs/toolkit";
import type { ICategory } from "../types/categoriesTypes";
import { apiCrudService } from "../../../shared/services/apiCrudService";

export const getAllCategoriesThunk = createAsyncThunk<ICategory[]>(
  "categories/getAll",
  async (_, { rejectWithValue }) => {
    try {
      const categories = await apiCrudService.getAll<ICategory>(
        "v1/Categories/Get-All",
      );
      return categories;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);
