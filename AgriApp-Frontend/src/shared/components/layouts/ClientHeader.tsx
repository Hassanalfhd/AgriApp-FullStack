// src/shared/components/Layout/ClientHeader.tsx
import { Link } from "react-router-dom";
import { useState } from "react";
import { clientLinks, type NavLink } from "@shared/constants/navbarLinks";
import { useAuth } from "@/shared/hooks/useAuth";
import UserMenu from "../ui/UserMenu";
import { usePermissionToDashoard } from "@/shared/hooks/usePermissionToDashoard";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { useAppSelector } from "@/shared/hooks/StoreHook";

interface ClientHeaderProps {
  onLogout?: () => void;
  username: string;
}

export default function ClientHeader({
  onLogout,
  username,
}: ClientHeaderProps) {
  const { cartCount: cartItemsCount } = useAppSelector((state) => state.cart);
  const isAuth = useAuth();
  const isHavePernissionToDashboard = usePermissionToDashoard();
  const [menuOpen, setMenuOpen] = useState(false);

  const renderLink = (link: NavLink) => {
    const showBadge = link.isCart && cartItemsCount > 0;

    return (
      <Link
        key={link.name}
        to={link.path}
        className="text-gray-700 hover:text-green-700 transition flex items-center gap-2 relative"
      >
        {link.icon}
        {link.name}
        {showBadge && (
          <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs w-5 h-5 rounded-full flex items-center justify-center">
            {cartItemsCount}
          </span>
        )}
      </Link>
    );
  };

  return (
    <header className="sticky top-0 z-10 flex items-center justify-between bg-white border-b px-4 py-3 shadow-sm relative">
      {/* Logo */}
      <Link to="/" className="text-xl font-bold text-green-700">
        AgriApp
      </Link>

      {/* Desktop Menu */}
      <nav className="hidden md:flex items-center gap-6">
        {clientLinks.map((link) => renderLink(link))}
        {isAuth && isHavePernissionToDashboard && (
          <Link
            key={`${ROUTES.ADMIN.DASHBOARD}`}
            className="text-gray-700 hover:text-green-700 transition flex items-center gap-2 relative"
            to={`${ROUTES.ADMIN.DASHBOARD}`}
          >
            Dashboard
          </Link>
        )}
        {!isAuth && (
          <Link
            to={`${ROUTES.CLIENT.LoginIn}`}
            className="text-gray-700 hover:text-green-700 transition"
          >
            Login
          </Link>
        )}
      </nav>

      {/* Mobile Menu Button */}
      <div className="md:hidden">
        <button
          onClick={() => setMenuOpen(!menuOpen)}
          className="text-green-700"
        >
          ☰
        </button>
      </div>

      {/* Mobile Dropdown */}
      {menuOpen && (
        <div className="absolute top-16 left-0 w-full bg-white shadow-md flex flex-col p-4 md:hidden gap-3 z-50">
          {clientLinks.map((link) => renderLink(link))}
          {isAuth && isHavePernissionToDashboard && (
            <Link
              key={`${ROUTES.ADMIN.DASHBOARD}`}
              className="text-gray-700 hover:text-green-700 transition flex items-center gap-2 relative"
              to={`${ROUTES.ADMIN.DASHBOARD}`}
            >
              Dashboard
            </Link>
          )}
          {!isAuth && (
            <Link
              to={`${ROUTES.CLIENT.LoginIn}`}
              className="text-gray-700 hover:text-green-700 transition"
            >
              Login
            </Link>
          )}
        </div>
      )}

      <UserMenu onLogout={onLogout} username={username} />
    </header>
  );
}
