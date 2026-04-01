import { createAsyncThunk } from "@reduxjs/toolkit";
import apiClient from "@/shared/services/apiClient";
import type { RootState } from "@/app/store";
import type { farmerOrderDto } from "../types/farmerOrderDtos";

export const getFramerOrders = createAsyncThunk<
  farmerOrderDto[],
  number,
  { state: RootState; rejectValue: string }
>("order/getFramerOrders", async (statusId, { rejectWithValue }) => {
  try {
    console.log(statusId);
    const baseUrl = "v1/Orders/my-orders";
    const url =
      statusId !== null ? `${baseUrl}?itemStatus=${statusId}` : baseUrl;
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
