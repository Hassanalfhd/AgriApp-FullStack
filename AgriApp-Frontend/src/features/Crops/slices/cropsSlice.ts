import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { CropsState, ICrops } from "../types/cropsTypes";
import { getAllCropsThunk } from "../actThunks/getAllCropsThunk";
import { addCropThunk } from "../actThunks/addCropThunk";
import { updateCropThunk } from "../actThunks/updateCropThunk";
import { deleteCropThunk } from "../actThunks/deleteCropThunk";
import { getCropByIdThunk } from "../actThunks/getCropByIdThunk";

const initialState: CropsState = {
  items: [],
  loading: "idle",
  selectedItem: null,
  error: null,
};

export const cropsSlice = createSlice({
  name: "categories",
  initialState,
  reducers: {
    deleteLocalCrop: (state, action) => {
      state.items = state.items.filter((p) => p.id !== action.payload);
    },
  },

  extraReducers: (builder) => {
    builder
      .addCase(getAllCropsThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        getAllCropsThunk.fulfilled,
        (state, action: PayloadAction<ICrops[]>) => {
          state.loading = "succeeded";
          state.items = action.payload;
        }
      )
      .addCase(getAllCropsThunk.rejected, (state, action) => {
        state.loading = "failed";
        if (action == null || action.payload == null) return;
        state.error = action.payload as string;
      });

    // Get Categroy By Id
    builder
      .addCase(getCropByIdThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
        state.selectedItem = null;
      })
      .addCase(
        getCropByIdThunk.fulfilled,
        (state, action: PayloadAction<ICrops>) => {
          if (state.selectedItem === action.payload) return;
          state.loading = "succeeded";
          state.selectedItem = action.payload;
        }
      )
      .addCase(getCropByIdThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Add Category
    builder
      .addCase(addCropThunk.pending, (state) => {
        state.loading = "pending";

        state.error = null;
      })
      .addCase(
        addCropThunk.fulfilled,
        (state, action: PayloadAction<ICrops>) => {
          state.loading = "succeeded";
          state.items.push(action.payload);
        }
      )
      .addCase(addCropThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Update Category
    builder
      .addCase(updateCropThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        updateCropThunk.fulfilled,
        (state, action: PayloadAction<ICrops>) => {
          if (!state.items.length) return;

          state.items.map((c) => {
            if (c.id === action.payload.id) {
              c = action.payload;
              state.loading = "succeeded";
              return;
            }
          });
        }
      )
      .addCase(updateCropThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Delete Category
    builder
      .addCase(deleteCropThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
        state.selectedItem = null;
      })
      .addCase(deleteCropThunk.fulfilled, (state) => {
        state.loading = "succeeded";
      })
      .addCase(deleteCropThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });
  },
});

export const { deleteLocalCrop } = cropsSlice.actions;

export default cropsSlice.reducer;
