import { lazy } from "react";
import DashboardLayout from "@/shared/components/layouts/DashboardLayout";
import ProtectedRoute from "./guards/ProtectedRoute";
import LoaderWrapper from "@/shared/components/ui/LoaderWrapper";
import { ROUTES } from "@/shared/constants/ROUTESLinks";

const AddEditCategory = lazy(
  () => import("@/features/categories/pages/dashboard/AddEditCategory"),
);

const ProductsPage = lazy(
  () => import("@/features/products/pages/dashboard/ProductsBoard"),
);
const CategoriesBoard = lazy(
  () => import("@features/categories/pages/dashboard/CategoriesBoard"),
);
const CropsPage = lazy(
  () => import("@features/Crops/pages/dashboard/CropsBoard"),
);
const Dashboard = lazy(() => import("@/features/Dashboard/DashboardPage"));
const CreateProductPage = lazy(
  () => import("@features/products/pages/CreateProductPage"),
);

const UsersPage = lazy(
  () => import("@features/users/pages/dashboard/UsersPage"),
);

const FarmerOrdersPage = lazy(
  () => import("@features/Orders/pages/Dashboard/FarmerOrdersPage"),
);

const AddEditCrop = lazy(
  () => import("@features/Crops/pages/dashboard/AddEditCrop"),
);

export const dashboardRoutes = {
  element: (
    <ProtectedRoute roles={["Admin", "Farmer"]}>
      <DashboardLayout />
    </ProtectedRoute>
  ),
  children: [
    {
      path: "/dashboard",
      element: (
        <ProtectedRoute roles={["Admin", "Farmer"]}>
          <LoaderWrapper>
            <Dashboard />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },

    {
      path: `${ROUTES.ADMIN.PRODUCTS.LIST}`,
      element: (
        <LoaderWrapper>
          <ProductsPage />
        </LoaderWrapper>
      ),
    },
    {
      path: `${ROUTES.ADMIN.CATEGORIES.LIST}`,
      element: (
        <LoaderWrapper>
          <CategoriesBoard />
        </LoaderWrapper>
      ),
    },

    {
      path: `${ROUTES.ADMIN.CROPS.LIST}`,
      element: (
        <LoaderWrapper>
          <CropsPage />
        </LoaderWrapper>
      ),
    },

    // Add crop only for specific roles
    {
      path: `${ROUTES.ADMIN.CROPS.ADD}`,
      element: (
        <ProtectedRoute roles={["Admin"]}>
          <LoaderWrapper>
            <AddEditCrop />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },

    // Edit crop only for specific roles
    {
      path: `${ROUTES.ADMIN.CROPS.EDITs}`,
      element: (
        <ProtectedRoute roles={["Admin"]}>
          <LoaderWrapper>
            <AddEditCrop />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },
    // Add product only for specific roles
    {
      path: `${ROUTES.ADMIN.PRODUCTS.ADD}`,
      element: (
        <ProtectedRoute roles={["Admin", "Farmer"]}>
          <LoaderWrapper>
            <CreateProductPage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },
    // Add product only for specific roles
    {
      path: `${ROUTES.ADMIN.PRODUCTS.EDIT}`,
      element: (
        <ProtectedRoute roles={["Admin", "Farmer"]}>
          <LoaderWrapper>
            <CreateProductPage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },

    // Add Category only for specific roles
    {
      path: `${ROUTES.ADMIN.CATEGORIES.ADD}`,
      element: (
        <ProtectedRoute roles={["Admin"]}>
          <LoaderWrapper>
            <AddEditCategory />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },
    // Edit Category only for specific roles
    {
      path: `${ROUTES.ADMIN.CATEGORIES.EDIT}`,
      element: (
        <ProtectedRoute roles={["Admin"]}>
          <LoaderWrapper>
            <AddEditCategory />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },

    // Users Page
    {
      path: `${ROUTES.ADMIN.USERS.LIST}`,
      element: (
        <ProtectedRoute roles={["Admin"]}>
          <LoaderWrapper>
            <UsersPage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },

    {
      path: `${ROUTES.FARMER.ORDERS}`,
      element: (
        <ProtectedRoute roles={["Admin", "Farmer"]}>
          <LoaderWrapper>
            <FarmerOrdersPage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },
  ],
};
