const DASHBOARD = "/dashboard";
const CLIENT = "/client";

export const ROUTES = {
  ADMIN: {
    DASHBOARD,
    PRODUCTS: {
      LIST: `${DASHBOARD}/products`,
      ADD: `${DASHBOARD}/products/add`,
      DETAILS: (id: string | number) => `${DASHBOARD}/products/${id}`,
      EDIT: `${DASHBOARD}/products/:id/edit`,
    },
    CROPS: {
      LIST: `${DASHBOARD}/crops`,
      ADD: `${DASHBOARD}/crops/add`,
      DETAILS: (id: string | number) => `${DASHBOARD}/crops/${id}`,
      EDIT: (id: string | number) => `${DASHBOARD}/crops/${id}/edit`,
      EDITs: `${DASHBOARD}/crops/:id/edit`,
    },
    CATEGORIES: {
      LIST: `${DASHBOARD}/categories`,
      ADD: `${DASHBOARD}/categories/add`,
      DETAILS: (id: string | number) => `${DASHBOARD}/categories/${id}`,
      EDIT: `${DASHBOARD}/categories/:id/edit`,
    },

    USERS: {
      LIST: `${DASHBOARD}/users`,
      ADD: `${DASHBOARD}/users/add`,
      DETAILS: (id: string | number) => `${DASHBOARD}/users/${id}`,
      EDIT: (id: string | number) => `${DASHBOARD}/users/${id}/edit`,
    },
    PURCHASES: `${DASHBOARD}/purchases`,
    NOTIFICATIONS: `${DASHBOARD}/notifications`,
  },

  FARMER: {
    DASHBOARD,
    MY_PRODUCTS: `${DASHBOARD}/products`,
    ORDERS: `${DASHBOARD}/orders`,
    NOTIFICATIONS: `${DASHBOARD}/notifications`,
  },

  CLIENT: {
    HOME: `${CLIENT}/home`,
    LoginIn: "/login",
    PROFILE: `${CLIENT}/profile`,
    CATEGORIES: `${CLIENT}/categories`,
    CROPS: `${CLIENT}/crops`,
    PRODUCTS: `${CLIENT}/products`,
    // PRODUCT_DETAILS: (id: string | number) => `${CLIENT}/products/`,
    ORDERS: `${CLIENT}/orders`,
    CART: `${CLIENT}/cart`,
  },
} as const;
