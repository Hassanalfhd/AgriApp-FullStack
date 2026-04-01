import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage";
import authReducer from "@/features/auth/slices/authSlice";
import productsReducer from "@/features/products/slices/productSlice";
import categoriesReducer from "@/features/categories/slices/categorySlice";
import CropsReducer from "@/features/Crops/slices/cropsSlice";
import UsersReducer from "@/features/users/slices/userSlice";
import CartReducer from "@/features/Orders/slices/cartSlice";
import customerOrdersReducer from "@/features/Orders/slices/customerOrdersSlice";
import FarmerOrdersReducer from "@/features/Orders/slices/farmerOrdersSlice";
import reportReducer from "@/features/reports/slices/reportSlice";

const rootReducer = combineReducers({
  auth: authReducer,
  products: productsReducer,
  categories: categoriesReducer,
  crops: CropsReducer,
  users: UsersReducer,
  cart: CartReducer,
  orders: customerOrdersReducer,
  FarmerOrders: FarmerOrdersReducer,
  reports: reportReducer,
});

const persistConfig = {
  key: "root",
  storage,
  whitelist: ["auth"],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

export const store = configureStore({
  reducer: persistedReducer,
  // if you want to ignored action in persist
  middleware: (gdm) =>
    gdm({
      serializableCheck: {
        ignoredActions: [
          "persist/PERSIST",
          "persist/REHYDRATE",
          "persist/PAUSE",
          "persist/FLUSH",
          "persist/PURGE",
          "persist/REGISTER",
        ],
      },
    }),
});

export const persistor = persistStore(store);
export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;
