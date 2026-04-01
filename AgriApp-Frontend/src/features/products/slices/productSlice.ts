import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type {
  ProductsFilters,
  ProductsState,
  TProduct,
  TProducts,
} from "../types/IProductsResponse";
// import { getProductsThunk } from "../actThunks/getProductsThunk";
import { getProductThunk } from "../actThunks/getProductThunk";
import { createProductWithImagesThunk } from "../actThunks/createProductWithImagesThunk";
import { deleteProductThunk } from "../actThunks/DeleteProduct";
import { getProductsImagesThunk } from "../actThunks/getProductsImagesThunk";
import type { ProductImageToUpdate } from "../types/ProductImageToUpdate";
import { updateProductWithImagesThunk } from "../actThunks/updateProductWithImagesThunk";
import { getProductsFilterThunk } from "../actThunks/getProductsFilterThunk";
import {
  getAdminProductsThunk,
  getProductsThunk,
} from "../actThunks/productsThunks";
import type { PagedResult } from "@/shared/types/PagedResult";
import { DEFAULT_PAGE_SIZE } from "@/shared/constants/Config";
import { getUserProductsThunk } from "../actThunks/getUserProductsThunk";
import { SearchSuggestionsOfProducts } from "../actThunks/SearchSuggestionsOfProducts";

// ---- Slice ----
const initialState: ProductsState = {
  Data: {
    items: [],
    page: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0,
  },
  loading: "idle",
  error: null,
  selectedItem: null,
  lastFetched: 0,
  lastFetchedProductId: "",
  images: [],
  filters: {
    minPrice: null,
    maxPrice: null,
    search: "",
  },

  hasMore: true,
  currentPage: 1,
  suggestions: [],
};

export const productsSlice = createSlice({
  name: "products",
  initialState,
  reducers: {
    clearProducts: (state) => {
      state.Data.items = [];
      state.loading = "idle";
      state.error = null;
      state.selectedItem = null;
      state.lastFetched = undefined;
    },

    deleteLocalProduct: (state, action) => {
      state.Data.items = state.Data.items.filter(
        (p) => p.id !== action.payload,
      );
    },

    // filters
    setFilter: (
      state,
      action: PayloadAction<{ name: keyof ProductsFilters; value: any }>,
    ) => {
      state.filters[action.payload.name] = action.payload.value;
    },

    resetFilters: (state) => {
      state.filters = initialState.filters;
    },
  },

  extraReducers: (builder) => {
    // --- getProductsThunk (clients) ---
    builder
      .addCase(getProductsThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        getProductsThunk.fulfilled,
        (state, action: PayloadAction<PagedResult<TProducts>>) => {
          state.loading = "succeeded";
          state.lastFetched = Date.now();
          const { items, totalCount } = action.payload; // البيانات القادمة من السيرفر
          // منع التكرار: نقارن IDs العناصر الجديدة بالعناصر الموجودة حالياً
          const existingIds = new Set(state.Data.items.map((item) => item.id));
          const uniqueNewItems = items.filter(
            (item) => !existingIds.has(item.id),
          );

          state.Data.items = [...state.Data.items, ...uniqueNewItems];

          const isLastPage = items.length < DEFAULT_PAGE_SIZE;
          const reachedTotal = state.Data.items.length >= totalCount;
          // تحديد هل توجد صفحات أخرى بناءً على إجمالي العدد من السيرفر
          state.hasMore = !isLastPage && !reachedTotal;

          state.Data.page = Math.ceil(items.length / state.Data.pageSize) + 1;
        },
      )
      .addCase(getProductsThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // --- getAdminProductsThunk (admin) ---
    builder.addCase(getAdminProductsThunk.pending, (state) => {
      state.loading = "pending";
      state.error = null;
    });
    builder.addCase(
      getAdminProductsThunk.fulfilled,
      (state, action: PayloadAction<PagedResult<TProducts>>) => {
        state.loading = "succeeded";
        // 1. تحديث البيانات (استبدال وليس دمج)
        state.Data = action.payload;
        state.currentPage = action.payload.page;
        state.lastFetched = Date.now();
      },
    );
    builder.addCase(getAdminProductsThunk.rejected, (state, action) => {
      state.loading = "failed";
      state.error = action.payload as string;
    });

    // get User Products
    builder.addCase(getUserProductsThunk.pending, (state) => {
      state.loading = "pending";
      state.error = null;
    });
    builder.addCase(
      getUserProductsThunk.fulfilled,
      (state, action: PayloadAction<PagedResult<TProducts>>) => {
        state.loading = "succeeded";
        // 1. تحديث البيانات (استبدال وليس دمج)
        state.Data = action.payload;
        state.currentPage = action.payload.page;
        state.lastFetched = Date.now();
      },
    );
    builder.addCase(getUserProductsThunk.rejected, (state, action) => {
      state.loading = "failed";
      state.error = action.payload as string;
    });

    // get product by id
    builder
      .addCase(getProductThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
        state.selectedItem = null;
      })
      .addCase(
        getProductThunk.fulfilled,
        (state, action: PayloadAction<TProduct>) => {
          if (state.selectedItem === action.payload) return;
          state.loading = "succeeded";
          state.selectedItem = action.payload;
          state.lastFetchedProductId = state.selectedItem.id.toString();
        },
      )
      .addCase(getProductThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Add new product
    builder
      .addCase(createProductWithImagesThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        createProductWithImagesThunk.fulfilled,
        (state, action: PayloadAction<TProducts>) => {
          state.loading = "succeeded";
          state.Data.items.push(action.payload);
        },
      );

    // Update Product

    builder
      .addCase(updateProductWithImagesThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        updateProductWithImagesThunk.fulfilled,
        (state, action: PayloadAction<TProducts>) => {
          if (!state.Data.items.length) return;
          state.Data.items.map((p) => {
            if (p.id === action.payload.id) {
              p = action.payload;
              state.loading = "succeeded";
              return;
            }
          });
          //  state.loading = "succeeded";
          //  const index = state.items.findIndex(
          //    (p) => p.id === action.payload.id,
          //  );
          //  if (index !== -1) {
          //    state.items[index] = action.payload;
          //  }
        },
      )
      .addCase(updateProductWithImagesThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Delete Product
    builder
      .addCase(deleteProductThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
        state.selectedItem = null;
      })
      .addCase(deleteProductThunk.fulfilled, (state) => {
        state.loading = "succeeded";
      })
      .addCase(deleteProductThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // get Images
    builder
      .addCase(getProductsImagesThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(
        getProductsImagesThunk.fulfilled,
        (state, action: PayloadAction<ProductImageToUpdate[]>) => {
          state.loading = "succeeded";
          state.images = action.payload;
        },
      )
      .addCase(getProductsImagesThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // Filters
    builder
      .addCase(getProductsFilterThunk.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(getProductsFilterThunk.fulfilled, (state, action) => {
        state.Data.items = action.payload;
        state.loading = "succeeded";
      })
      .addCase(getProductsFilterThunk.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });

    // SearchSuggestionsOfProducts
    builder
      .addCase(SearchSuggestionsOfProducts.pending, (state) => {
        state.loading = "pending";
        state.error = null;
      })
      .addCase(SearchSuggestionsOfProducts.fulfilled, (state, action) => {
        state.suggestions = action.payload;
        state.loading = "succeeded";
      })
      .addCase(SearchSuggestionsOfProducts.rejected, (state, action) => {
        state.loading = "failed";
        state.error = action.payload as string;
      });
  },
});

export const { clearProducts, deleteLocalProduct, setFilter, resetFilters } =
  productsSlice.actions;
export default productsSlice.reducer;
