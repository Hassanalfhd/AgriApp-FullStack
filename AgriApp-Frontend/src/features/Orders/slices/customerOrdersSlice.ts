import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type {
  orderItemsResponseDto,
  OrderResponseDto,
  OrderState,
} from "../types/orderDto";
import { sendOrder } from "../actThunks/sendOrder";
import { getUserOrders } from "../actThunks/getUserOrders";
import { canceledOrder } from "../actThunks/canceledOrder";
import { canceledOrderItem } from "../actThunks/canceledOrderItem";

const initialState: OrderState = {
  orders: [],
  isLoading: "pending",
  isSuccess: false,
  error: null,
  lastOrderId: null,
};

const customerOrdersSlice = createSlice({
  name: "order",
  initialState,
  reducers: {
    // دالة لتصفير الحالة (مثلاً عند الانتقال لصفحة جديدة)
    resetOrderState: (state) => {
      state.isLoading = "idle";
      state.isSuccess = false;
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // 1. أثناء الإرسال
      .addCase(sendOrder.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      // 2. في حال النجاح
      .addCase(
        sendOrder.fulfilled,
        (state, action: PayloadAction<{ id: string }>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          state.lastOrderId = action.payload.id;
        },
      )
      // 3. في حال الفشل
      .addCase(sendOrder.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });

    // get user orders
    builder
      // 1. أثناء الإرسال
      .addCase(getUserOrders.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      // 2. في حال النجاح
      .addCase(
        getUserOrders.fulfilled,
        (state, action: PayloadAction<OrderResponseDto[]>) => {
          state.isLoading = "succeeded";
          state.orders = action.payload || [];
        },
      )
      .addCase(getUserOrders.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(canceledOrder.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      .addCase(
        canceledOrder.fulfilled,
        (state, action: PayloadAction<number>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          const orderindex = state.orders.findIndex(
            (o) => o.orderId === action.payload,
          );

          state.orders[orderindex].status = "Cancelled";
        },
      )
      .addCase(canceledOrder.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(canceledOrderItem.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      .addCase(
        canceledOrderItem.fulfilled,
        (state, action: PayloadAction<orderItemsResponseDto>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          const orderIndex = state.orders.findIndex(
            (o) => o.orderId === action.payload.orderId,
          );

          if (orderIndex !== -1) {
            state.orders[orderIndex].items = state.orders[orderIndex].items.map(
              (item) =>
                item.orderItemId === action.payload.orderItemId
                  ? action.payload
                  : item,
            );
          }
        },
      )
      .addCase(canceledOrderItem.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });
  },
});

export const { resetOrderState } = customerOrdersSlice.actions;
export default customerOrdersSlice.reducer;
