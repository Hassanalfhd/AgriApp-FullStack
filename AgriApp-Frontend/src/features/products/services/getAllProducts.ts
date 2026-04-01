import apiClient from "@/shared/services/apiClient";
import type { TProducts } from "../types/IProductsResponse";
import type { PagedResult } from "@/shared/types/PagedResult";

export const getProducts = async (
  page = 1,
  pageSize = 10,
): Promise<PagedResult<TProducts>> => {
  const res = await apiClient.get<PagedResult<TProducts>>(
    `v1/products/Page?page=${page}&pageSize=${pageSize}`,
  );
  return res.data;
};

export const getAdminProducts = async (
  page = 1,
  pageSize = 10,
): Promise<PagedResult<TProducts>> => {
  const res = await apiClient.get<PagedResult<TProducts>>(
    `v1/products/adminPage?page=${page}&pageSize=${pageSize}`,
  );
  return res.data;
};
