import { createSlice } from "@reduxjs/toolkit";
import type { UserState } from "../types/userTypes";
import { getAllUsersThunk } from "../actThunks/getAllUsersThunk";
import { toggleUserStatusThunk } from "../actThunks/toggleUserStatusThunk";

const initialState: UserState = {
  data: {
    items: [],
    page: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0,
  },
  loading: "idle",
  error: null,
};

const userSlice = createSlice({
  name: "users",
  initialState,
  reducers: {
    clearUsersState: (state) => {
      state.data.items = [];
      state.data.page = 1;
      state.data.pageSize = 5;
      state.data.totalCount = 0;
      state.data.totalPages = 0;
      state.error = null;
      state.loading = "idle";
    },
  },
  extraReducers: (builder) => {
    // ========================
    // Fetch Users
    // ========================
    builder
      .addCase(getAllUsersThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(getAllUsersThunk.fulfilled, (state, action) => {
        state.loading = "succeeded";
        state.data.items = action.payload.items;
        state.data.page = action.payload.page;
        state.data.totalCount = action.payload.totalCount;
        state.data.totalPages = action.payload.totalPages;
      })
      .addCase(getAllUsersThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    builder
      .addCase(toggleUserStatusThunk.pending, (state, action) => {
        // اختياري: يمكنك وضع حالة تحميل فرعية لكل مستخدم إذا أردت
      })
      .addCase(toggleUserStatusThunk.fulfilled, (state, action) => {
        const { userId, isActive } = action.payload;
        const user = state.data.items.find((u) => u.id === userId);
        if (user) {
          user.isActive = isActive;
        }
      })
      .addCase(toggleUserStatusThunk.rejected, (state, action) => {
        state.error = action.payload as string;
      });
  },
});

export const { clearUsersState } = userSlice.actions;

export default userSlice.reducer;
