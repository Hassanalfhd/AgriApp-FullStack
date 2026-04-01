import type { enLoadinStatus } from "@/shared/types/enums/enLoadin";
import type { enRoles } from "@/shared/types/enums/enRoles";

export interface IUserDto {
  id?: number;
  email: string;
  fullName: string;
  username: string;
  phone?: string;
  userType: string;
  createdAt?: Date;
}

export type IUser = IUserDto & {
  role?: enRoles;
  imageFile?: string;
};

export type TUserToRegiser = IUserDto & { password: string };

export interface ILoginRequest {
  email: string;
  password: string;
}

export interface ILoginResponse {
  token: string;
  isSuccess: boolean;
  error: string | null;
  message: string | null;
}

export interface IAuthDto {
  user: IUser | null;
  token: string | null;
  loading: enLoadinStatus;
  error: string | null;
  isAuthenticated: boolean;
}
