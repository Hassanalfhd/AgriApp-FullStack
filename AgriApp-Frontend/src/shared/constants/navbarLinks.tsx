import { Home, Box, Users, ShoppingCart, Bell } from "lucide-react";
import type { enRoles } from "../types/enums/enRoles";
import { ROUTES } from "./ROUTESLinks";

export interface NavLink {
  name: string;
  path: string;
  icon?: React.ReactNode;
  roles?: string[];
  isCart?: boolean;
}

/* ---------------------- Admin Links ---------------------- */
export const adminLinks: NavLink[] = [
  { name: "Dashboard", path: ROUTES.ADMIN.DASHBOARD, icon: <Home size={18} /> },

  {
    name: "Products",
    path: ROUTES.ADMIN.PRODUCTS.LIST,
    icon: <Box size={18} />,
  },
  { name: "Crops", path: ROUTES.ADMIN.CROPS.LIST, icon: <Box size={18} /> },
  {
    name: "Categories",
    path: ROUTES.ADMIN.CATEGORIES.LIST,
    icon: <Box size={18} />,
  },
  { name: "Users", path: ROUTES.ADMIN.USERS.LIST, icon: <Users size={18} /> },
  {
    name: "Purchases",
    path: ROUTES.ADMIN.PURCHASES,
    icon: <ShoppingCart size={18} />,
  },
  {
    name: "Orders",
    path: ROUTES.FARMER.ORDERS,
    icon: <ShoppingCart size={18} />,
  },
  {
    name: "Notifications",
    path: ROUTES.ADMIN.NOTIFICATIONS,
    icon: <Bell size={18} />,
  },
];

/* ---------------------- Farmer Links ---------------------- */
export const farmerLinks: NavLink[] = [
  {
    name: "Dashboard",
    path: ROUTES.FARMER.DASHBOARD,
    icon: <Home size={18} />,
  },
  {
    name: "My Products",
    path: ROUTES.FARMER.MY_PRODUCTS,
    icon: <Box size={18} />,
  },
  {
    name: "Orders",
    path: ROUTES.FARMER.ORDERS,
    icon: <ShoppingCart size={18} />,
  },
  {
    name: "Notifications",
    path: ROUTES.FARMER.NOTIFICATIONS,
    icon: <Bell size={18} />,
  },
];

/* ---------------------- Client Links ---------------------- */
export const clientLinks: NavLink[] = [
  { name: "Products", path: ROUTES.CLIENT.PRODUCTS },
  { name: "Crops", path: ROUTES.CLIENT.CROPS },
  { name: "Categoies", path: ROUTES.CLIENT.CATEGORIES },
  { name: "Orders", path: ROUTES.CLIENT.ORDERS },
  {
    name: "Cart",
    path: ROUTES.CLIENT.CART,
    icon: <ShoppingCart size={18} />,
    isCart: true,
  },
];

/* ---------------------- Role Filter ---------------------- */
export const getLinksByRole = (role: enRoles): NavLink[] => {
  switch (role) {
    case "Admin":
      return adminLinks;
    case "Farmer":
      return farmerLinks;
    case "Customer":
      return clientLinks;
    default:
      return [];
  }
};
