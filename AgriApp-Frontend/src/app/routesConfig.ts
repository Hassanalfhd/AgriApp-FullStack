// src/routes/routesConfig.ts
import { lazy } from "react";

const HomePage = lazy(() => import("@features/HomePage"));
const ProductsPage = lazy(
  () => import("@features/products/pages/ProductsPage"),
);
const ProductDetails = lazy(
  () => import("@/features/products/pages/ProductDetailsPage"),
);
const CreateProductPage = lazy(
  () => import("@features/products/pages/CreateProductPage"),
);
const CategoriesPage = lazy(
  () => import("@features/categories/pages/CategoriesPage"),
);
const CropsPage = lazy(() => import("@features/Crops/pages/CropsPage"));
const LoginPage = lazy(() => import("@/features/auth/pages/LoginPage"));

export const guestRoutes = [
  { path: "/", element: HomePage },
  { path: "/products", element: ProductsPage },
  { path: "/products/:productId", element: ProductDetails },
  { path: "/categories", element: CategoriesPage },
  { path: "/crops", element: CropsPage },
  { path: "/login", element: LoginPage },
];

export const clientRoutes = [
  { path: "/client", element: HomePage },
  { path: "/client/products", element: ProductsPage },
  { path: "/client/products/:productId", element: ProductDetails },
  { path: "/client/categories", element: CategoriesPage },
  { path: "/client/crops", element: CropsPage },
];

export const dashboardRoutes = [
  { path: "/dashboard", element: HomePage },
  { path: "/dashboard/products", element: ProductsPage },
  {
    path: "/dashboard/products/add",
    element: CreateProductPage,
    roles: ["Admin", "Farmer"],
  },
  { path: "/dashboard/categories", element: CategoriesPage },
  { path: "/dashboard/crops", element: CropsPage },
];
