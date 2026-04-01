import apiClient from "../../../shared/services/apiClient";
import type { ICrops } from "../types/cropsTypes";

export const getAllCropsServices = async (): Promise<ICrops[]> => {
  try {
    const response = await apiClient.get<ICrops[]>("v1/crops/All");
    return response.data;
  } catch (error: any) {
    throw new Error(
      error?.response?.data?.message ||
        error.message ||
        "Error fetching categories"
    );
  }
};
