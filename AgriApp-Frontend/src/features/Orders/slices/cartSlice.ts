import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { CartItems, CartState } from "../types/cartDto";
import toast from "react-hot-toast";

// دالة مساعدة لحساب الإجمالي والعدد وتحديث localStorage
const updateLocalStorage = (state: CartState) => {
  state.totalAmount = state.items.reduce(
    (total, item) => total + item.price * item.quantity,
    0,
  );
  state.cartCount = state.items.length;
  localStorage.setItem("cartItems", JSON.stringify(state.items));
};

// محاولة استعادة البيانات من localStorage عند بدء التطبيق
const savedItems = localStorage.getItem("cartItems");
const parsedItems: CartItems[] = savedItems ? JSON.parse(savedItems) : [];

const initialState: CartState = {
  items: parsedItems,
  userId: 0,
  // نحسب الإجماليات بناءً على ما تم استعادته
  totalAmount: parsedItems.reduce(
    (total, item) => total + item.price * item.quantity,
    0,
  ),
  cartCount: parsedItems.length,
};

const cartSlice = createSlice({
  name: "cart",
  initialState,
  reducers: {
    addToCart: (state, action: PayloadAction<CartItems>) => {
      const newItem = action.payload;

      // ملاحظة: البحث عادة يكون بـ productId فقط، إلا إذا كانت لديك مواصفات أخرى
      const existingItem = state.items.find(
        (item) => item.productId === newItem.productId,
      );

      if (existingItem) {
        // إذا كان المنتج موجود، نزيد الكمية فقط
        existingItem.quantity += newItem.quantity;
      } else {
        // إذا كان منتج جديد، نضيفه للمصفوفة
        state.items.push({ ...newItem });
      }

      // تحديث الإجماليات والـ localStorage
      updateLocalStorage(state);
    },

    removeFromCart: (state, action: PayloadAction<number | string>) => {
      // نستخدم productId أو id حسب ما ترسله من الواجهة
      state.items = state.items.filter(
        (item) => item.productId !== action.payload,
      );

      updateLocalStorage(state);
    },

    clearCart: (state) => {
      state.items = [];
      state.totalAmount = 0;
      state.cartCount = 0;
      localStorage.removeItem("cartItems");
    },
  },
});

export const { addToCart, removeFromCart, clearCart } = cartSlice.actions;
export default cartSlice.reducer;
