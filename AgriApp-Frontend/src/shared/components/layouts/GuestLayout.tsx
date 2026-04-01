// src/shared/components/Layout/GuestLayout.tsx
import { Link, Outlet } from "react-router-dom";

export default function GuestLayout() {
  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      {/* Navbar بسيط للزائر */}
      <header className="flex items-center justify-between bg-white border-b px-4 py-3 shadow-sm">
        <Link to="/" className="text-xl font-bold text-green-700">
          AgriApp
        </Link>
        <nav className="flex gap-4">
          <Link
            to="/products"
            className="text-gray-700 hover:text-green-700 transition"
          >
            Products
          </Link>
          <Link
            to="/login"
            className="text-gray-700 hover:text-green-700 transition"
          >
            Login
          </Link>
          <Link
            to="/register"
            className="text-gray-700 hover:text-green-700 transition"
          >
            Register
          </Link>
        </nav>
      </header>
      <main className="flex-1 p-4 sm:p-6 md:p-8">
        <Outlet />
      </main>
    </div>
  );
}
