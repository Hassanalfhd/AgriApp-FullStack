import apiClient from "@/shared/services/apiClient";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { CartItems } from "../types/cartDto";

// دالة إرسال السلة كاملة إلى الـ API
export const checkoutOrder = createAsyncThunk(
  "order/checkout",
  async (
    cartData: { items: CartItems[]; total: number },
    { rejectWithValue },
  ) => {
    try {
      const response = await apiClient.post(
        "/api/orders/isPlacedBefore",
        cartData,
      );
      return response.data;
    } catch (err: any) {
      return rejectWithValue(err.response.data);
    }
  },
);
