import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import { getFramerOrders } from "../actThunks/getFramerOrders";
import type {
  farmerOrderDto,
  farmerOrderState,
} from "../types/farmerOrderDtos";
import { accetedOrderItem } from "../actThunks/accetedOrderItem";
import { cancelledOrderItemByFarmer } from "../actThunks/cancelledOrderItemByFarmer";
import { pickUpOrderItem } from "../actThunks/pickUpOrderItem";

const initialState: farmerOrderState = {
  items: [],
  isLoading: "idle",
  isSuccess: false,
  error: null,
  lastOrderId: null,
};

const farmerOrdersSlice = createSlice({
  name: "order",
  initialState,
  reducers: {
    resetOrderState: (state) => {
      state.isLoading = "idle";
      state.isSuccess = false;
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(getFramerOrders.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      .addCase(
        getFramerOrders.fulfilled,
        (state, action: PayloadAction<farmerOrderDto[]>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          state.items = action.payload || [];
        },
      )
      .addCase(getFramerOrders.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(accetedOrderItem.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      .addCase(
        accetedOrderItem.fulfilled,
        (state, action: PayloadAction<number>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          const orderItemindex = state.items.findIndex(
            (oi) => oi.orderItemId === action.payload,
          );

          state.items[orderItemindex].status = "Accepted";
        },
      )
      .addCase(accetedOrderItem.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(cancelledOrderItemByFarmer.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      .addCase(
        cancelledOrderItemByFarmer.fulfilled,
        (state, action: PayloadAction<number>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          const orderItemindex = state.items.findIndex(
            (oi) => oi.orderItemId === action.payload,
          );
          state.items[orderItemindex].status = "Cancelled";
        },
      )
      .addCase(cancelledOrderItemByFarmer.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });

    builder
      .addCase(pickUpOrderItem.pending, (state) => {
        state.isLoading = "pending";
        state.error = null;
        state.isSuccess = false;
      })
      .addCase(
        pickUpOrderItem.fulfilled,
        (state, action: PayloadAction<number>) => {
          state.isLoading = "succeeded";
          state.isSuccess = true;
          const orderItemindex = state.items.findIndex(
            (oi) => oi.orderItemId === action.payload,
          );
          state.items[orderItemindex].status = "ReadyForPickup";
        },
      )
      .addCase(pickUpOrderItem.rejected, (state, action) => {
        state.isLoading = "failed";
        state.isSuccess = false;
        state.error = action.payload as string;
      });
  },
});

export const { resetOrderState } = farmerOrdersSlice.actions;
export default farmerOrdersSlice.reducer;
