import type { PagedResult } from "@/shared/types/PagedResult";
import apiClient from "@/shared/services/apiClient";
import type { UserResponseDto } from "../types/userTypes";

export const getAllUsers = async (
  page = 1,
  pageSize = 5,
): Promise<PagedResult<UserResponseDto>> => {
  const res = await apiClient.get<PagedResult<UserResponseDto>>(
    `v1/users/usersPage?page=${page}&pageSize=${pageSize}`,
  );

  return res.data;
};
