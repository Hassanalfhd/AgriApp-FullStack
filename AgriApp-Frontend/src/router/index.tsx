import { createBrowserRouter } from "react-router-dom";
import { publicRoutes } from "./public.routes";
import { clientRoutes } from "./client.routes";
import { dashboardRoutes } from "./dashboard.routes";
// import ErrorLayout from "@/shared/layouts/ErrorLayout";
import NotFoundPage from "@/shared/pages/NotFoundPage";

export const router = createBrowserRouter([
  publicRoutes,
  clientRoutes,
  dashboardRoutes,
  { path: "*", element: <NotFoundPage /> },
]);
