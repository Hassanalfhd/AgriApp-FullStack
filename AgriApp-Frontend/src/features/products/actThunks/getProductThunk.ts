// getProductThunk.ts
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { TProduct } from "../types/IProductsResponse";
import { apiCrudService } from "../../../shared/services/apiCrudService";
export const getProductThunk = createAsyncThunk<
  TProduct, // Return type
  number | string, // Argument type (productId)
  { rejectValue: string } // Rejection type
>("products/getProductById", async (productId, { rejectWithValue }) => {
  try {
    const id = typeof productId === "string" ? parseInt(productId) : productId;
    const product = await apiCrudService.getById<TProduct>("v1/products", id);

    return product;
  } catch (err: any) {
    let message = "Error";
    if (err instanceof Error) message = err.message;
    if ((err as any)?.response?.data?.message)
      message = (err as any).response.data.message;
    return rejectWithValue(message);
  }
});
