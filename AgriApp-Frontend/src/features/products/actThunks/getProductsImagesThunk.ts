import { createAsyncThunk } from "@reduxjs/toolkit";
import { apiCrudService } from "@/shared/services/apiCrudService";
import type { ProductImageToUpdate } from "../types/ProductImageToUpdate";

export const getProductsImagesThunk = createAsyncThunk<
  ProductImageToUpdate[],
  number | string,
  { rejectValue: string }
>("products/getProductImages", async (productId, { rejectWithValue }) => {
  try {
    const id = typeof productId === "string" ? parseInt(productId) : productId;

    const images = await apiCrudService.getAll<ProductImageToUpdate>(
      `v1/products/images/${id}`
    );

    return images; // ← يرجع القائمة كاملة
  } catch (err: any) {
    return rejectWithValue(err?.response?.data?.message || "فشل في جلب الصور");
  }
});
