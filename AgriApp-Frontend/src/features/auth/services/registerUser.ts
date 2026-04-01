import apiClient from "@/shared/services/apiClient";
import type { TUserToRegiser } from "../types/authDto";

export const registerUser = async (user: TUserToRegiser) => {
  const response = await apiClient.post("v1/auth/register", user, {});

  return response.data;
};
