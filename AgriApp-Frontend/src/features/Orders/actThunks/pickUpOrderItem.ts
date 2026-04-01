import apiClient from "@/shared/services/apiClient";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const pickUpOrderItem = createAsyncThunk<
  number,
  number,
  { rejectValue: string }
>("order/pickUpOrderItem", async (orderItemId: number, { rejectWithValue }) => {
  try {
    const baseUrl = "v1/Orders/pickup";
    const url = orderItemId !== null ? `${baseUrl}?id=${orderItemId}` : baseUrl;
    const res = await apiClient.patch(url);
    return res.data.data;
  } catch (err: any) {
    return rejectWithValue(err.response.data);
  }
});
