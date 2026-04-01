import apiClient from "@/shared/services/apiClient";
import type { IUser } from "../types/authDto";

export async function getProfileService() {
  try {
    const response = await apiClient.get("v1/users/profile");

    return response.data;
  } catch (error: any) {
    //  التعامل مع الخطأ بشكل واضح
    if (error.response && error.response.data) {
      throw new Error(error.response.data.message || "Data is Getting failed");
    }
    throw new Error(error.message || "Network error");
  }
}


export const updateMyProfile = async (formData: FormData) => {
  const { data } = await apiClient.put<IUser>("v1/users/profile/me", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
  return data;
};
