import { lazy } from "react";
import ClientLayout from "@/shared/components/layouts/ClientLayout";
import ProtectedRoute from "./guards/ProtectedRoute";
import LoaderWrapper from "@/shared/components/ui/LoaderWrapper";
import { ROUTES } from "@/shared/constants/ROUTESLinks";

const ProductsPage = lazy(
  () => import("@/features/products/pages/ProductsPage"),
);
const ProductDetails = lazy(
  () => import("@/features/products/pages/ProductDetailsPage"),
);
const CategoriesPage = lazy(
  () => import("@features/categories/pages/CategoriesPage"),
);
const CropsPage = lazy(() => import("@features/Crops/pages/CropsPage"));
const HomePage = lazy(() => import("@features/HomePage"));
const CartPage = lazy(() => import("@features/Orders/pages/CartPage"));
const OrderPage = lazy(() => import("@features/Orders/pages/OrdersPage"));
const ProfilePage = lazy(() => import("@features/auth/pages/ProfilePage"));
export const clientRoutes = {
  element: (
    <ProtectedRoute>
      <ClientLayout />
    </ProtectedRoute>
  ),
  children: [
    { path: `${ROUTES.CLIENT.HOME}`, element: <HomePage /> },

    {
      path: `${ROUTES.CLIENT.PRODUCTS}`,
      element: (
        <LoaderWrapper>
          <ProductsPage />
        </LoaderWrapper>
      ),
    },

    {
      path: "/client/products/:productId",
      element: (
        <LoaderWrapper>
          <ProductDetails />
        </LoaderWrapper>
      ),
    },

    {
      path: `${ROUTES.CLIENT.CATEGORIES}`,
      element: (
        <LoaderWrapper>
          <CategoriesPage />
        </LoaderWrapper>
      ),
    },

    {
      path: `${ROUTES.CLIENT.PROFILE}`,
      element: (
        <ProtectedRoute
          roles={["Admin", "Farmer", "AgriculturalExpert", "Customer"]}
        >
          <LoaderWrapper>
            <ProfilePage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },
    {
      path: `${ROUTES.CLIENT.CROPS}`,
      element: (
        <LoaderWrapper>
          <CropsPage />
        </LoaderWrapper>
      ),
    },

    {
      path: `${ROUTES.CLIENT.CART}`,
      element: (
        <ProtectedRoute roles={["Admin", "Customer"]}>
          <LoaderWrapper>
            <CartPage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },

    {
      path: `${ROUTES.CLIENT.ORDERS}`,
      element: (
        <ProtectedRoute roles={["Admin", "Customer"]}>
          <LoaderWrapper>
            <OrderPage />
          </LoaderWrapper>
        </ProtectedRoute>
      ),
    },
  ],
};
