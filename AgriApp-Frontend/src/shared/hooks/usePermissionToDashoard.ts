import { useAppSelector } from "./StoreHook";

export const usePermissionToDashoard = (): boolean => {
  const user = useAppSelector((s) => s.auth.user);
  if (!user || !user.role || user.role === "Customer" || user.role === "Guest")
    return false;

  return true;
};
