import type { enLoadinStatus } from "@/shared/types/enums/enLoadin";

export interface OrderPayload {
  userId: number;
  items: orderItems[];
}

export interface orderItems {
  orderItemId: number;
  productId: number;
  quantity: number;
  status: number;
}

export interface OrderState {
  orders: OrderResponseDto[];
  isLoading: enLoadinStatus;
  isSuccess: boolean;
  error: string | null;
  lastOrderId: string | null;
}

export interface OrderResponseDto {
  items: orderItemsResponseDto[];
  orderId: number;
  status: orderStatus;
  totalAmount: number;
  createdAt: Date;
}

export interface orderItemsResponseDto {
  productId: number;
  productName: string;
  orderItemId: number;
  farmerId: number;
  quantity: number;
  orderId?: number;
  status: itemStatus;
  unitPrice: number;
  total: number;
}

export type orderStatus =
  | "Pending"
  | "Confirmed"
  | "PartiallyShipped"
  | "Completed"
  | "Cancelled";

export type itemStatus =
  | "Pending"
  | "Accepted"
  | "ReadyForPickup"
  | "Shipped"
  | "Cancelled";
