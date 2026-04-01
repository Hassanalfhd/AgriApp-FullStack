import { useAppSelector } from "@/shared/hooks/StoreHook";
import type { enRoles } from "@/shared/types/enums/enRoles";
import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";

export default function RoleGuard({
  roles,
  children,
}: {
  roles: enRoles[];
  children: ReactNode;
}) {
  const { user } = useAppSelector((s) => s.auth.user);

  console.log("adfkjdsf",user)
  if (roles && user?.role) {
    const allowedRoles = Array.isArray(roles) ? roles : [roles];
    if (!allowedRoles.includes(user.role)) {
      return <Navigate to="/403" replace />;
    }
  }

  return children;
}
