import { Navigate } from "react-router-dom";
import { useAuth } from "@/shared/hooks/useAuth";
import type { ReactNode } from "react";

export default function AuthRedirect({ children }: { children: ReactNode }) {
  const isAuthenticated = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/client/home" replace />;
  }

  return children;
}
