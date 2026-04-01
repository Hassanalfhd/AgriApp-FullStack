import apiClient from "@/shared/services/apiClient";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { orderItemsResponseDto } from "../types/orderDto";

export const canceledOrderItem = createAsyncThunk<
  orderItemsResponseDto,
  number,
  { rejectValue: string }
>(
  "order/canceledOrderItem",
  async (orderItemId: number, { rejectWithValue }) => {
    try {
      const res = await apiClient.patch(`v1/orders/cancel-Item/${orderItemId}`);
      return res.data.data;
    } catch (err: any) {
      return rejectWithValue(err.response.data);
    }
  },
);
