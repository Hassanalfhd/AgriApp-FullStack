import { createAsyncThunk } from "@reduxjs/toolkit";
import apiClient from "@/shared/services/apiClient";

export const SearchSuggestionsOfProducts = createAsyncThunk(
  "products/ProductsSearchSuggestions",
  async (val, { rejectWithValue }) => {
    try {
      console.log(val);
      const res = await apiClient.get(
        `v1/Products/SearchSuggestions?query=${val}`,
      );

      console.log(res.data);

      return res.data;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  },
);
