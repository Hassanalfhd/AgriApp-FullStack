import type { RootState } from "@/app/store";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { TProducts } from "../types/IProductsResponse";
import apiClient from "@/shared/services/apiClient";

export const getProductsFilterThunk = createAsyncThunk(
  "products/getProductsFilter",
  async (_, { getState, rejectWithValue }) => {
    try {
      const { filters } = (getState() as RootState).products;

      const params: Record<string, any> = {};

      if (filters.search) params.search = filters.search;
      if (filters.minPrice != null) params.minPrice = filters.minPrice;
      if (filters.maxPrice != null) params.maxPrice = filters.maxPrice;

      const response = await apiClient.get("v1/products/filters", { params });
      return response.data as TProducts[];
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  },
);
