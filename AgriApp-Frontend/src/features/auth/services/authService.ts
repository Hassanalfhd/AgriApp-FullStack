import apiClient from "../../../shared/services/apiClient";
import type { ILoginRequest, ILoginResponse } from "../types/authDto";

export async function loginService(
  payload: ILoginRequest,
): Promise<ILoginResponse> {
  try {
    const response = await apiClient.post<ILoginResponse>(
      "v1/auth/login",
      payload,
    );

    return response.data;
  } catch (error: any) {
    //  التعامل مع الخطأ بشكل واضح
    if (error.response && error.response.data) {
      throw new Error(error.response.data.message || "Login failed");
    }
    throw new Error(error.message || "Network error");
  }
}
