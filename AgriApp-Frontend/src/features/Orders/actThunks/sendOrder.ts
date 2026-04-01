import { createAsyncThunk } from "@reduxjs/toolkit";
import type { OrderPayload } from "../types/orderDto";
import apiClient from "@/shared/services/apiClient";

export const sendOrder = createAsyncThunk(
  "order/sendOrder",
  async (orderData: OrderPayload, { rejectWithValue }) => {
    try {
      const response = await apiClient.post("v1/Orders", orderData);
      // لنفترض أن السيرفر يعيد بيانات الطلب مع ID
      return response.data;
    } catch (err: any) {
      // إرجاع رسالة الخطأ من السيرفر
      return rejectWithValue(
        err.response?.data?.message ||
          "An error occurred while sending the request. Please try again later.",
      );
    }
  },
);
