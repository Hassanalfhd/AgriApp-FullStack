import { ChevronsLeft, ChevronsRight, X } from "lucide-react";
import Sidebar from "@shared/components/ui/Sidebar";
import { layoutClasses } from "@shared/constants/classNames";
import LogoutJSX from "../ui/LogoutJSX";
import { Link } from "react-router-dom";
import { ROUTES } from "@/shared/constants/ROUTESLinks";

interface DashboardSidebarProps {
  collapsed: boolean;
  setCollapsed: (value: boolean) => void;
  sidebarOpen: boolean;
  setSidebarOpen: (value: boolean) => void;
  links: any[];
  onLogout?: () => void;
}

export default function DashboaSrdSidebar({
  collapsed,
  setCollapsed,
  sidebarOpen,
  setSidebarOpen,
  links,
  onLogout,
}: DashboardSidebarProps) {
  return (
    <aside
      className={`${layoutClasses.sidebar.base} ${
        sidebarOpen ? layoutClasses.sidebar.open : layoutClasses.sidebar.closed
      } ${
        collapsed
          ? layoutClasses.sidebar.collapsed
          : layoutClasses.sidebar.expanded
      }`}
    >
      {/* Sidebar Header */}
      <div className="flex items-center justify-between p-3 border-b border-green-600">
        <Link
          to={ROUTES.CLIENT.HOME}
          className={`${
            collapsed ? "hidden lg:block" : "block font-bold text-lg"
          }`}
        >
          AgriApp
        </Link>
        <div className="flex items-center gap-2">
          <button
            onClick={() => setCollapsed(!collapsed)}
            className=" lg:inline-flex p-2 mt-1   rounded hover:bg-green-600/80"
          >
            {collapsed ? (
              <ChevronsRight size={18} />
            ) : (
              <ChevronsLeft size={18} />
            )}
          </button>
          <button
            onClick={() => setSidebarOpen(false)}
            className="lg:hidden p-1 rounded hover:bg-green-600/80"
          >
            <X size={18} />
          </button>
        </div>
      </div>

      {/* Sidebar Links */}
      <Sidebar
        links={links}
        collapsed={collapsed}
        onLinkClick={() => setSidebarOpen(false)}
      />

      {/* Logout */}
      <LogoutJSX
        className="flex items-center px-3 gap-2 text-sm hover:text-gray-200 transition w-full"
        onLogout={onLogout}
      />
    </aside>
  );
}
