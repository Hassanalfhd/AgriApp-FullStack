import type { PagedResult } from "@/shared/types/PagedResult";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { UserResponseDto } from "../types/userTypes";
import { getAllUsers } from "../services/getAllUsers";

export const getAllUsersThunk = createAsyncThunk<
  PagedResult<UserResponseDto>, // returned data
  { page?: number; pageSize?: number } | undefined // params
>("users/getAllUsers", async (params, { rejectWithValue }) => {
  try {
    const { page = 1, pageSize = 5 } = params || {};
    return await getAllUsers(page, pageSize);
  } catch (err: any) {
    return rejectWithValue(err.response?.data || "Failed to fetch products");
  }
});
