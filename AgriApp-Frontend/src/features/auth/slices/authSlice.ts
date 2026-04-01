import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";
import type { IAuthDto, ILoginResponse, IUser } from "../types/authDto";
import { login } from "../actThunk/loginThunk";
import { profile } from "../actThunk/getUserProfile";
import { registerUserThunk } from "../actThunk/registerUserThunk";
import { updateProfileThunk } from "../actThunk/updateProfileThunk";
import type { enRoles } from "@/shared/types/enums/enRoles";
import { User } from "lucide-react";

const initialState: IAuthDto = {
  user: null,
  error: null,
  loading: "idle",
  token: null,
  isAuthenticated: false,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    logout: (state) => {
      state.error = null;
      state.token = null;
      state.isAuthenticated = false;
      state.user = null;
    },

    setToken: (state, action) => {
      state.token = action.payload;
    },

    setCredentials: (
      state,
      action: PayloadAction<{ user: IUser | null; token: string }>,
    ) => {
      state.user = action.payload.user;
      state.token = action.payload.token;
      state.isAuthenticated = !!action.payload;
    },
  },

  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (s) => {
        s.loading = "pending";
        s.error = null;
      })
      .addCase(login.fulfilled, (s, action: PayloadAction<ILoginResponse>) => {
        s.loading = "succeeded";
        s.token = action.payload.token;
      })
      .addCase(login.rejected, (s, action) => {
        s.loading = "failed";
        if (typeof action.payload == "string") s.error = action.payload;
      });

    builder
      .addCase(profile.pending, (s) => {
        s.loading = "pending";
        s.error = null;
      })
      .addCase(profile.fulfilled, (s, action: PayloadAction<IUser>) => {
        s.loading = "succeeded";
        s.user = action.payload;
        s.user.role = (action.payload.userType as enRoles) ?? "Guest";
      })
      .addCase(profile.rejected, (s, action) => {
        s.loading = "failed";
        if (typeof action.payload == "string") s.error = action.payload;
      });

    builder
      .addCase(updateProfileThunk.pending, (state) => {
        state.loading = "pending";
      })

      .addCase(updateProfileThunk.fulfilled, (state, action) => {
        state.loading = "succeeded";
        state.user = action.payload;
        state.user.id = action.payload.id;
      })
      .addCase(updateProfileThunk.rejected, (state, action) => {
        state.loading = "failed";
        if (typeof action.payload === "string") state.error = action.payload;
      });

    // register
    builder
      .addCase(registerUserThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(registerUserThunk.fulfilled, (state, action) => {
        state.loading = "succeeded";
        state.user = action.payload;
      })
      .addCase(registerUserThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.error.message || "Failed to register";
      });
  },
});

export const { logout, setCredentials, setToken } = authSlice.actions;

export default authSlice.reducer;
