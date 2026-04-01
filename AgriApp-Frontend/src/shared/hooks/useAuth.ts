import { useAppSelector } from "./StoreHook";

export const useAuth = (): boolean => {
  const token = useAppSelector((s) => s.auth.token);
  if (!token) return false;
  return true;
};
