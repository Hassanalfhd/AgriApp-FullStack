import { createAsyncThunk } from "@reduxjs/toolkit";
import apiClient from "@/shared/services/apiClient";
import type { OrderResponseDto } from "../types/orderDto";
import type { RootState } from "@/app/store";

export const getUserOrders = createAsyncThunk<
  OrderResponseDto[],
  string,
  { state: RootState; rejectValue: string }
>("order/getUserOrders", async (status, { rejectWithValue }) => {
  try {
    let statusNo = 0;
    if (status === "pending") statusNo = 1;
    if (status === "cancelled") statusNo = 5;
    if (status === "completed") statusNo = 4;

    const url = status ? `v1/orders?status=${statusNo}` : "/orders";
    const response = await apiClient.get(url);
    return response.data.data;
  } catch (err: any) {
    // إرجاع رسالة الخطأ من السيرفر
    return rejectWithValue(
      err.response?.data?.message ||
        "An error occurred while sending the request. Please try again later.",
    );
  }
});
