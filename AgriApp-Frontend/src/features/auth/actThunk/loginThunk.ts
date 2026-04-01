import { createAsyncThunk } from "@reduxjs/toolkit";
import type { ILoginRequest, ILoginResponse } from "../types/authDto";
import { loginService } from "../services/authService";
import { isAxiosError } from "axios";
import { setCredentials } from "../slices/authSlice";

export const login = createAsyncThunk<ILoginResponse, ILoginRequest>(
  "auth/login",

  async (payload, thunkAPI) => {
    try {
      const res = await loginService(payload);
      thunkAPI.dispatch(setCredentials({ user: null, token: res.token }));

      return res;
    } catch (err) {
      if (isAxiosError(err))
        return thunkAPI.rejectWithValue(err.response?.data?.message);

      return thunkAPI.rejectWithValue("Unacepted error!");
    }
  },
);
