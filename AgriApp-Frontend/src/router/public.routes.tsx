import { lazy } from "react";
import GuestLayout from "@/shared/components/layouts/GuestLayout";
import AuthRedirect from "./guards/AuthRedirect";
import LoaderWrapper from "@/shared/components/ui/LoaderWrapper";

const HomePage = lazy(() => import("@features/HomePage"));
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
const LoginPage = lazy(() => import("@/features/auth/pages/LoginPage"));
const RegisterPage = lazy(() => import("@/features/auth/pages/RegisterPage"));

export const publicRoutes = {
  element: (
    <AuthRedirect>
      <GuestLayout />
    </AuthRedirect>
  ),
  children: [
    {
      index: true,
      path: "/",
      element: (
        <LoaderWrapper>
          <HomePage />
        </LoaderWrapper>
      ),
    },
    { path: "/login", element: <LoginPage /> },
    { path: "/register", element: <RegisterPage /> },

    {
      path: "/products",
      element: (
        <LoaderWrapper>
          <ProductsPage />
        </LoaderWrapper>
      ),
    },
    {
      path: "/products/:productId",
      element: (
        <LoaderWrapper>
          <ProductDetails />
        </LoaderWrapper>
      ),
    },

    {
      path: "/categories",
      element: (
        <LoaderWrapper>
          <CategoriesPage />
        </LoaderWrapper>
      ),
    },

    {
      path: "/crops",
      element: (
        <LoaderWrapper>
          <CropsPage />
        </LoaderWrapper>
      ),
    },
  ],
};
