import { createAsyncThunk } from "@reduxjs/toolkit";
import type { IUser } from "../types/authDto";
import { isAxiosError } from "axios";
import { getProfileService } from "../services/profileService";

export const profile = createAsyncThunk<IUser>(
  "auth/user/profile",
  async (_, thunkAPI) => {
    try {
      const user: IUser = await getProfileService();
      return user;
    } catch (err) {
      if (isAxiosError(err))
        return thunkAPI.rejectWithValue(err.response?.data?.message);
      return thunkAPI.rejectWithValue("Unacepted error!");
    }
  },
);
