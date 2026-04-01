import type { PagedResult } from "@/shared/types/PagedResult";
import type { TProducts } from "../types/IProductsResponse";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { RootState } from "@/app/store";
import { getUserProducts } from "../services/getUserProducts";

export const getUserProductsThunk = createAsyncThunk<
  PagedResult<TProducts>, // returned data
  { page?: number; pageSize?: number } | undefined // params
>("products/getUserProducts", async (params, { rejectWithValue, getState }) => {
  try {
    const state = getState() as RootState;
    const userId = state.auth.user?.id;
    const { page = 1, pageSize = 10 } = params || {};
    return await getUserProducts(page, pageSize, userId);
  } catch (err: any) {
    return rejectWithValue(err.response?.data || "Failed to fetch products");
  }
});
