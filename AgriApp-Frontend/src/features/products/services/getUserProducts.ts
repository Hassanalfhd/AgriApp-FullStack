import type { PagedResult } from "@/shared/types/PagedResult";
import type { TProducts } from "../types/IProductsResponse";
import apiClient from "@/shared/services/apiClient";


export const getUserProducts = async (
  page = 1,
  pageSize = 10,
  userId = -1,
): Promise<PagedResult<TProducts>> => {
  const res = await apiClient.get<PagedResult<TProducts>>(
    `v1/products/UserProducts?page=${page}&pageSize=${pageSize}&userId=${userId}`,
  );

  return res.data;
};
