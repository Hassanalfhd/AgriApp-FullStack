import type { enLoadinStatus } from "@/shared/types/enums/enLoadin";
import type { PagedResult } from "@/shared/types/PagedResult";

export interface userProfile {
  id: number;
  fullName: string;
  username: string;
  email: string;
  userType: number;
  createdAt: string;
}

export interface UserResponseDto {
  id: number;
  fullName: string;
  username: string;
  email: string;
  phone: string;
  userType: string;
  isActive: boolean;
  imageFIle: string;
  createdAt: Date;
}

export interface UserState {
  data: PagedResult<UserResponseDto>;
  loading: enLoadinStatus;
  error: string | null;
}
