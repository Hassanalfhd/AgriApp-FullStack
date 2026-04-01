import apiClient from "../../../shared/services/apiClient";
import type { TProducts } from "../types/IProductsResponse";

export const getProductsService = async (): Promise<TProducts[]> => {
  try {
    const response = await apiClient.get<TProducts[]>("v1/products/All");

    return response.data;
  } catch (error: any) {
    throw new Error(
      error?.response?.data?.message ||
        error.message ||
        "Error fetching products",
    );
  }
};
