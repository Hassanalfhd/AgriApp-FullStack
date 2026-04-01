// src/shared/components/Layout/ClientLayout.tsx
// import { type ReactNode } from "react";
import { Outlet } from "react-router-dom";
import ClientHeader from "./ClientHeader";
import Footer from "../ui/Footer";
import { useLogout } from "@/shared/hooks/useLogout";
import { useAppSelector } from "@/shared/hooks/StoreHook";

export default function ClientLayout() {
  const handleLogout = useLogout();
  const username = useAppSelector((s) => s.auth.user?.username);
  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <ClientHeader
        username={typeof username === "string" ? username : ""}
        onLogout={handleLogout}
      />
      <main className="flex-1 p-4 sm:p-6 md:p-8">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}
