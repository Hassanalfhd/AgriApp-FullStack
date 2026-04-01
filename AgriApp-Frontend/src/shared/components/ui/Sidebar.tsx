// src/shared/components/UI/Sidebar.tsx
import { NavLink as RouterNavLink, useLocation } from "react-router-dom";
import { layoutClasses } from "../../constants/classNames";
import { type NavLink as NavLinkType } from "@shared/constants/navbarLinks";

interface SidebarProps {
  links: NavLinkType[];
  collapsed: boolean;
  onLinkClick?: () => void;
}

export default function Sidebar({
  links,
  collapsed,
  onLinkClick,
}: SidebarProps) {
  const location = useLocation();

  return (
    <nav className="flex-1 flex flex-col mt-4">
      {links.map((link) => {
        const isActive = location.pathname === link.path;
        return (
          <RouterNavLink
            key={link.name}
            to={link.path}
            onClick={onLinkClick}
            className={`
              ${layoutClasses.sidebar.link} 
              ${isActive ? "bg-green-600/90" : ""} 
              ${collapsed ? "justify-center px-0" : ""}
            `}
          >
            {link.icon}
            {!collapsed && <span>{link.name}</span>}
          </RouterNavLink>
        );
      })}
    </nav>
  );
}
