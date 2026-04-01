import type { enLoadinStatus } from "@/shared/types/enums/enLoadin";
import type { itemStatus } from "./orderDto";

export interface farmerOrderDto {
  orderItemId: number;
  productName: string;
  quantity: number;
  customerName: string;
  status: itemStatus;
  orderDate: Date;
  unitPrice: number;
  total: number;
}

export interface farmerOrderState {
  items: farmerOrderDto[];
  isLoading: enLoadinStatus;
  isSuccess: boolean;
  error: string | null;
  lastOrderId: string | null;
}
