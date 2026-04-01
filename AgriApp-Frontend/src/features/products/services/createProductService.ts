import apiClient from "../../../shared/services/apiClient";
import type { TProducts } from "../types/IProductsResponse";
import type { IProductToCreate } from "../types/ProductToCreate";

// not used we repleced by apiCrudService
export async function createProductService(product: IProductToCreate) {
  try {
    const response = await apiClient.post("v1/products/AddNewProduct", product);
    return response.data.data as TProducts;
  } catch (error: any) {
    console.error("Get Product By Id Error:", error);
    throw error;
  }
}
