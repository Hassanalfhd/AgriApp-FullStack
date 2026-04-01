import type { enRoles } from "../types/enums/enRoles";
import { useAppSelector } from "./StoreHook";

export const useUserRole = (): enRoles => {
  const user = useAppSelector((s) => s.auth.user);
  if (user != null) {
    if (user.role) return user.role;
  }
  return "Guest";
};
