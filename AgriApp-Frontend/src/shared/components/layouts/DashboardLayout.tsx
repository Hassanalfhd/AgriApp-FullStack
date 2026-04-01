import { useState } from "react";
import { Outlet } from "react-router-dom";
import BaseDashboardLayout from "@shared/components/layouts/BaseDashboardLayout";
import DashboardHeader from "@shared/components/layouts/DashboardHeader";
import DashboardSidebar from "@shared/components/layouts/DashboardSidebar";
import { useLogout } from "@shared/hooks/useLogout";
import { getLinksByRole } from "@shared/constants/navbarLinks";
import Footer from "../ui/Footer";
import { layoutClasses } from "@/shared/constants/classNames";
import { useAppSelector } from "@/shared/hooks/StoreHook";

export default function DashboardLayout() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [collapsed, setCollapsed] = useState(false);
  const handleLogout = useLogout();
  const { username, role } = useAppSelector((s) => s.auth?.user);

  const links = getLinksByRole(role);

  return (
    <BaseDashboardLayout
      sidebar={
        <DashboardSidebar
          collapsed={collapsed}
          setCollapsed={setCollapsed}
          sidebarOpen={sidebarOpen}
          setSidebarOpen={setSidebarOpen}
          links={links}
          onLogout={handleLogout}
        />
      }
      header={
        <DashboardHeader
          sidebarOpen={sidebarOpen}
          setSidebarOpen={setSidebarOpen}
          username={username}
          onLogout={handleLogout}
        />
      }
    >
      <main
        className={`${layoutClasses.main.default} 
          ${
            collapsed
              ? layoutClasses.main.collapsed
              : layoutClasses.main.expanded
          }

        
        `}
      >
        <Outlet />
      </main>

      <Footer />
    </BaseDashboardLayout>
  );
}
