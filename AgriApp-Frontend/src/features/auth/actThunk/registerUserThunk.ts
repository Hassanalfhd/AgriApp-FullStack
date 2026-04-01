import { createAsyncThunk } from "@reduxjs/toolkit";
import { registerUser } from "../services/registerUser";
import type { IUserDto, TUserToRegiser } from "../types/authDto";

export const registerUserThunk = createAsyncThunk<
  IUserDto, // ✅ نوع البيانات الراجعة عند النجاح
  TUserToRegiser, // ✅ نوع البيانات المرسلة
  { rejectValue: string } // ✅ نوع الخطأ (اختياري لكن موصى به)
>("user/register", async (user, thunkAPI) => {
  try {
    const response = await registerUser(user);
    return response as IUserDto;
  } catch (error: any) {
    return thunkAPI.rejectWithValue(
      error?.response?.data?.message || "Register failed"
    );
  }
});
