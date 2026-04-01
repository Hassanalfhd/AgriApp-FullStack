import apiClient from "../../../shared/services/apiClient";
import type { TProduct } from "../types/IProductsResponse";

export const getProductById = async (id: number): Promise<TProduct> => {
  try {
    const response = await apiClient.get(`v1/products/${id}`);
    return response.data.data;
  } catch (error: any) {
    throw new Error(
      error?.response?.data?.message ||
        error.message ||
        "Error fetching products"
    );
  }
};
