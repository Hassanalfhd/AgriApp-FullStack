import { RouterProvider } from "react-router-dom";
import "./App.css";
import Toast from "./shared/components/ui/Toast";
import { router } from "./router/index";
// import { useEffect, useState } from "react";
// import { setToken } from "./features/auth/slices/authSlice";
// import { useDispatch } from "react-redux";
// import apiClient from "./shared/services/apiClient";
// import Loader from "./shared/components/Loader";

export default function App() {
  // const dispatch = useDispatch();
  // const [loading, setLoading] = useState(true);
  // useEffect(() => {
  //   const initAuth = async () => {
  //     try {
  //       // محاولة جلب Access Token جديد باستخدام الـ Refresh Cookie الموجود أصلاً
  //       const response = await apiClient.post("v1/auth/refresh-token");
  //       dispatch(setToken(response.data.token));
  //     } catch (err) {
  //       console.log("No active session found.");
  //     } finally {
  //       setLoading(false);
  //     }
  //   };
  //   initAuth();
  // }, [dispatch]);

  // if (loading) return <Loader />;

  return (
    <>
      <RouterProvider router={router} />
      <Toast />
    </>
  );
}
