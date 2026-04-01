import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { CategoriesState, ICategory } from "../types/categoriesTypes";
import { getAllCategoriesThunk } from "../actThunks/getAllCategoriesThunk";
import { deleteCategoryThunk } from "../actThunks/deleteCategoryThunk";
import { createNewCategoryThunk } from "../actThunks/createNewCategoryThunk";
import { updateCagegoryThunk } from "../actThunks/updateCagegoryThunk";
import { getCategoryByIdThunk } from "../actThunks/getCategoryByIdThunk";

const initialState: CategoriesState = {
  items: [],
  loading: "idle",
  selectedItem: null,
  error: null,
};

export const categorySlice = createSlice({
  name: "categories",
  initialState,
  reducers: {
    cleanUp: () => {},

    deleteLocalCategory: (state, action) => {
      state.items = state.items.filter((p) => p.id !== action.payload);
    },
  },

  extraReducers: (builder) => {
    // Get All Categories
    builder
      .addCase(getAllCategoriesThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        getAllCategoriesThunk.fulfilled,
        (state, action: PayloadAction<ICategory[]>) => {
          state.loading = "succeeded";
          state.items = action.payload;
        }
      )
      .addCase(getAllCategoriesThunk.rejected, (state, action) => {
        state.loading = "failed";
        if (action == null || action.payload == null) return;
        state.error = action.payload as string;
      });

    // Get Categroy By Id
    builder
      .addCase(getCategoryByIdThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
        state.selectedItem = null;
      })
      .addCase(
        getCategoryByIdThunk.fulfilled,
        (state, action: PayloadAction<ICategory>) => {
          if (state.selectedItem === action.payload) return;
          state.loading = "succeeded";
          state.selectedItem = action.payload;
        }
      )
      .addCase(getCategoryByIdThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Add Category
    builder
      .addCase(createNewCategoryThunk.pending, (state) => {
        state.loading = "pending";

        state.error = null;
      })
      .addCase(
        createNewCategoryThunk.fulfilled,
        (state, action: PayloadAction<ICategory>) => {
          state.loading = "succeeded";
          state.items.push(action.payload);
        }
      )
      .addCase(createNewCategoryThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

      
    // Update Category
    builder
      .addCase(updateCagegoryThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        updateCagegoryThunk.fulfilled,
        (state, action: PayloadAction<ICategory>) => {
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
      .addCase(updateCagegoryThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Delete Category
    builder
      .addCase(deleteCategoryThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
        state.selectedItem = null;
      })
      .addCase(deleteCategoryThunk.fulfilled, (state) => {
        state.loading = "succeeded";
      })
      .addCase(deleteCategoryThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });
  },
});

export const { deleteLocalCategory } = categorySlice.actions;

export default categorySlice.reducer;
