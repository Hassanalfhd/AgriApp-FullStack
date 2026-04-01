// getProductThunk.ts
import { createAsyncThunk } from "@reduxjs/toolkit";
import { apiCrudService } from "@/shared/services/apiCrudService";
export const deleteProductThunk = createAsyncThunk<
  number, // Return type
  number | string, // Argument type (productId)
  { rejectValue: string } // Rejection type
>("products/Delete", async (productId, { rejectWithValue }) => {
  try {
    const id = typeof productId === "string" ? parseInt(productId) : productId;
    await apiCrudService.delete("v1/products", id);
    return id;
  } catch (err: any) {
    let message = "Error";
    if (err instanceof Error) message = err.message;
    if ((err as any)?.response?.data?.message)
      message = (err as any).response.data.message;
    return rejectWithValue(message);
  }
});
