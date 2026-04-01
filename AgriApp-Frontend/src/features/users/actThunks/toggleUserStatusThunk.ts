import apiClient from "@/shared/services/apiClient";
import { createAsyncThunk } from "@reduxjs/toolkit";

interface ToggleStatusArgs {
  userId: string | number;
  isActive: boolean;
}


export const toggleUserStatusThunk = createAsyncThunk(
  "users/toggleStatus",
  async ({ userId, isActive }: ToggleStatusArgs, { rejectWithValue }) => {
    try {
      // بناءً على وصفك للـ Endpoint: /{userId}/status
      const response = await apiClient.patch(`v1/users/${userId}/status`, {
        isActive: isActive,
      });

      return { userId, isActive: response.data.isActive ?? isActive };
    } catch (error: any) {
      // إرجاع رسالة الخطأ في حال فشل الطلب
      return rejectWithValue(
        error.response?.data?.message || "Failed to update user status",
      );
    }
  },
);
