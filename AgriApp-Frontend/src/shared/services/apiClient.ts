// apiClient.ts
import axios from "axios";
import { store } from "../../app/store";
import { logout, setToken } from "@/features/auth/slices/authSlice";
import { ROUTES } from "../constants/ROUTESLinks";

const apiClient = axios.create({
  baseURL: "https://localhost:7170/api",
  withCredentials: true, // مهم لإرسال HttpOnly Cookie
});

// ✓ Automatically add token from Redux to headers
apiClient.interceptors.request.use((config) => {
  const state = store.getState();
  const token = state.auth.token;
  if (token) config.headers.Authorization = `${token}`;
  return config;
});

// ✓ Global response handler with refresh token support
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) prom.reject(error);
    else prom.resolve(token);
  });
  failedQueue = [];
};

apiClient.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest = error.config;

    if (originalRequest.url.includes(`${ROUTES.CLIENT.LoginIn}`)) return Promise.reject(error);
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        // إذا كان هناك طلب تجديد جاري بالفعل، انتظر حتى ينتهي
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers.Authorization = `${token}`;
            return apiClient(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      originalRequest._retry = true;
      isRefreshing = true;

      return new Promise((resolve, reject) => {
        apiClient
          .post("v1/auth/refresh-token")
          .then((res) => {
            const newToken = res.data.token;

            store.dispatch(setToken(newToken));
            processQueue(null, newToken);

            apiClient.defaults.headers.common["Authorization"] = ` ${newToken}`;
            originalRequest.headers.Authorization = `${newToken}`;

            resolve(apiClient(originalRequest));
          })
          .catch((err) => {
            processQueue(err, null);
            store.dispatch(logout());
            reject(err);
          })
          .finally(() => {
            isRefreshing = false;
          });
      });
    }

    return Promise.reject(error.response?.data.message || "Server Error");
  },
);

export default apiClient;
