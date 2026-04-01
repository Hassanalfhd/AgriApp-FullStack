// getProductThunk.ts
import { createAsyncThunk } from "@reduxjs/toolkit";
import { apiCrudService } from "@/shared/services/apiCrudService";
import type { ICrops } from "../types/cropsTypes";
export const deleteCropThunk = createAsyncThunk<
  ICrops, // Return type
  number | string, // Argument type (productId)
  { rejectValue: string } // Rejection type
>("Crops/DeleteCrop", async (cropsId, { rejectWithValue }) => {
  try {
    const id = typeof cropsId === "string" ? parseInt(cropsId) : cropsId;
    const category = await apiCrudService.delete<ICrops>("v1/Crops", id);

    return category;
  } catch (err: any) {
    let message = "Error";
    if (err instanceof Error) message = err.message;
    if ((err as any)?.response?.data?.message)
      message = (err as any).response.data.message;
    return rejectWithValue(message);
  }
});
