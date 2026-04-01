import { Menu, X } from "lucide-react";
import UserMenu from "@/shared/components/ui/UserMenu";

interface DashboardHeaderProps {
  sidebarOpen: boolean;
  setSidebarOpen: (value: boolean) => void;
  username: string;
  onLogout?: () => void;
}

export default function DashboardHeader({
  sidebarOpen,
  setSidebarOpen,
  username,
  onLogout,
}: DashboardHeaderProps) {
  return (
    <header
      className="sticky top-0 z-20 flex items-center justify-between bg-white border-b border-gray-100 px-4 sm:px-6 py-3 shadow-sm"
      role="banner"
    >
      <div className="flex items-center gap-4">
        {/* Mobile Sidebar Toggle */}
        <button
          onClick={() => setSidebarOpen(!sidebarOpen)}
          className="lg:hidden p-2 -ml-2 text-gray-600 hover:bg-gray-50 rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-blue-500"
          aria-expanded={sidebarOpen}
          aria-controls="main-sidebar" // Ensure your Sidebar has id="main-sidebar"
          aria-label={
            sidebarOpen ? "Close navigation menu" : "Open navigation menu"
          }
        >
          {sidebarOpen ? (
            <X size={24} aria-hidden="true" />
          ) : (
            <Menu size={24} aria-hidden="true" />
          )}
        </button>

        {/* Dashboard Title */}
        <h1 className="font-bold text-xl text-gray-900 tracking-tight">
          Dashboard
        </h1>
      </div>

      {/* Right Side: User Menu and Actions */}
      <div className="flex items-center gap-2">
        <div className="hidden sm:block mr-2">
          <span className="text-xs font-medium text-gray-400 uppercase tracking-widest">
            Welcome back
          </span>
        </div>

        {/* UserMenu likely contains a button/dropdown */}
        <nav aria-label="User account actions">
          <UserMenu username={username} onLogout={onLogout} />
        </nav>
      </div>
    </header>
  );
}
