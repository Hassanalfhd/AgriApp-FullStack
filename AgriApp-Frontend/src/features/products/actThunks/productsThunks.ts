// src/store/products/productsThunks.ts
import { createAsyncThunk } from "@reduxjs/toolkit";
import { getAdminProducts, getProducts } from "../services/getAllProducts";
import type { PagedResult } from "@/shared/types/PagedResult";
import type { TProducts } from "../types/IProductsResponse";

export const getProductsThunk = createAsyncThunk<
  PagedResult<TProducts>, // نوع البيانات المرجعة
  { page?: number; pageSize?: number } | undefined // params
>("products/getProducts", async (params, { rejectWithValue }) => {
  try {
    const { page = 1, pageSize = 10 } = params || {};
    return await getProducts(page, pageSize);
  } catch (err: any) {
    return rejectWithValue(err.response?.data || "Failed to fetch products");
  }
});

// للمسؤول (admin)
export const getAdminProductsThunk = createAsyncThunk<
  PagedResult<TProducts>,
  { page?: number; pageSize?: number } | undefined
>("products/getAdminProducts", async (params, { rejectWithValue }) => {
  try {
    const { page = 1, pageSize = 20 } = params || {};
    return await getAdminProducts(page, pageSize);
  } catch (err: any) {
    return rejectWithValue(
      err.response?.data || "Failed to fetch admin products",
    );
  }
});
