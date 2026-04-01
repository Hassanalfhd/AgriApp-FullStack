import type { PagedResult } from "@/shared/types/PagedResult";
import type { enLoadinStatus } from "../../../shared/types/enums/enLoadin";
import type { ProductImageToUpdate } from "./ProductImageToUpdate";

export interface IProduct {
  id: number;
  name: string;
  description?: string;
  price: number;
  quantityName?: string;
  cropId?: number;
  createdBy?: number;
  createdByName?: string;
  createdAt?: Date;
  quantityInStock?: number;
  cropTypeId?: number;
  createdByImage: string;
  quantityTypeId?: number;
}

export type TProducts = IProduct & { image: string };

export type TProduct = IProduct & { cropName: string; images: string[] };

export interface IProductsResponse {
  isSuccess: boolean;
  data: TProducts[];
  message?: string;
}

export interface ProductsState {
  Data: PagedResult<TProducts>;
  loading: enLoadinStatus;
  error: string | null;
  selectedItem: TProduct | null;
  lastFetched?: number;
  images: ProductImageToUpdate[];
  lastFetchedProductId: string;
  filters: ProductsFilters;
  hasMore?: boolean;
  currentPage: number;
  suggestions?: [];
}

export interface ProductsFilters {
  minPrice?: number | null;
  maxPrice?: number | null;
  search?: string;
}
