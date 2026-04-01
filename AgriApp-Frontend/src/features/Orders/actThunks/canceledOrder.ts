import apiClient from "@/shared/services/apiClient";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const canceledOrder = createAsyncThunk<
  number,
  number,
  { rejectValue: string }
>("order/canceledOrder", async (orderId: number, { rejectWithValue }) => {
  try {
    const res = await apiClient.patch(`v1/orders/cancel/${orderId}`);
    return res.data.data;
  } catch (err: any) {
    return rejectWithValue(err.response.data);
  }
});
