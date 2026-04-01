import apiClient from "@/shared/services/apiClient";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const cancelledOrderItemByFarmer = createAsyncThunk<
  number,
  number,
  { rejectValue: string }
>(
  "order/cancelledOrderItemByFarmer",
  async (orderItemId: number, { rejectWithValue }) => {
    try {
      const res = await apiClient.patch(
        `v1/orders/farmer/cancel-Item/${orderItemId}`,
      );
      return res.data.data;
    } catch (err: any) {
      return rejectWithValue(err.response.data);
    }
  },
);
