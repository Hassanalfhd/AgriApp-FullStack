// getProductThunk.ts
import { createAsyncThunk } from "@reduxjs/toolkit";
import { apiCrudService } from "../../../shared/services/apiCrudService";
import type { ICategory } from "../types/categoriesTypes";
export const deleteCategoryThunk = createAsyncThunk<
  ICategory, // Return type
  number | string, // Argument type (productId)
  { rejectValue: string } // Rejection type
>("categories/Delete", async (categoryId, { rejectWithValue }) => {
  try {
    const id =
      typeof categoryId === "string" ? parseInt(categoryId) : categoryId;
    const category = await apiCrudService.delete<ICategory>(
      "v1/Categories/Delete",
      id
    );

    return category;
  } catch (err: any) {
    let message = "Error";
    if (err instanceof Error) message = err.message;
    if ((err as any)?.response?.data?.message)
      message = (err as any).response.data.message;
    return rejectWithValue(message);
  }
});
