import { useState, useRef, useEffect } from "react";
import { User, Settings, LogOut } from "lucide-react";
import { NavLink } from "react-router-dom";
import { ROUTES } from "@/shared/constants/ROUTESLinks";

interface UserMenuProps {
  username: string;
  onLogout?: () => void;
}

export default function UserMenu({ username, onLogout }: UserMenuProps) {
  const [isOpen, setIsOpen] = useState(false);
  const ref = useRef<HTMLDivElement | null>(null);

  // يغلق القائمة عند النقر خارجها
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent | TouchEvent) => {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside, true);
    document.addEventListener("touchstart", handleClickOutside, true);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside, true);
      document.removeEventListener("touchstart", handleClickOutside, true);
    };
  }, [setIsOpen]);

  return (
    <div className="relative" ref={ref}>
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="flex items-center gap-2 bg-green-600 text-white px-3 py-2 rounded-full text-sm font-medium hover:bg-green-700 transition"
      >
        <User size={18} />
        <span className="hidden sm:inline">{username}</span>
      </button>

      {isOpen && (
        <div className="absolute right-0 mt-2 w-44 bg-white border rounded-lg shadow-md z-50">
          <NavLink
            to={ROUTES.CLIENT.PROFILE}
            className="flex items-center gap-2 px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
            onClick={() => setIsOpen(false)}
          >
            <User size={16} /> Profile
          </NavLink>
          <NavLink
            to="/dashboard/settings"
            className="flex items-center gap-2 px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
            onClick={() => setIsOpen(false)}
          >
            <Settings size={16} /> Settings
          </NavLink>
          <button
            onClick={() => {
              setIsOpen(false);
              if (onLogout) onLogout();
              else alert("Logged out"); // placeholder
            }}
            className="flex items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-gray-100 w-full text-left"
          >
            <LogOut size={16} /> Logout
          </button>
        </div>
      )}
    </div>
  );
}
