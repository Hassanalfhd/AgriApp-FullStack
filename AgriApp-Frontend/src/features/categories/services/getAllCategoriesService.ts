import apiClient from "../../../shared/services/apiClient";
import type { ICategory } from "../types/categoriesTypes";

// we change this for apiServices ==> global with getnaric type

export const getAllCategoriesService = async (): Promise<ICategory[]> => {
  try {
    const response = await apiClient.get<ICategory[]>("v1/Categories/Get-All");
    return response.data;
  } catch (error: any) {
    throw new Error(
      error?.response?.data?.message ||
        error.message ||
        "Error fetching categories"
    );
  }
};
