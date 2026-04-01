import type { ReactNode } from "react";
import { useAppSelector } from "@shared/hooks/StoreHook";
import { Navigate } from "react-router-dom";
import { type enRoles } from "@shared/types/enums/enRoles";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import ForbiddenPage from "@/shared/pages/ForbiddenPage";
// import jwtDecode from 'jwt-decode';

interface IProps {
  children: ReactNode;
  roles?: enRoles | enRoles[]; // تحسين لقبول أكثر من دور
}

export default function ProtectedRoute({ children, roles }: IProps) {
  const { token, user } = useAppSelector((s) => s.auth);

  if (!token) return <Navigate to={`${ROUTES.CLIENT.LoginIn}`} replace />;

  if (roles && user?.role) {
    const allowedRoles = Array.isArray(roles) ? roles : [roles];
    if (!allowedRoles.includes(user.role)) {
      return <ForbiddenPage />;
    }
  }

  // we uesd when the toekn is expired.
  // try {
  //   const decoded: any = jwtDecode(token);
  //   const isExpired = decoded.exp * 1000 < Date.now();
  //   if (isExpired) return <Navigate to="/login" replace />;
  // } catch {
  //   return <Navigate to="/login" replace />;
  // }

  // return <Outlet/>;
  return <>{children}</>;
}
