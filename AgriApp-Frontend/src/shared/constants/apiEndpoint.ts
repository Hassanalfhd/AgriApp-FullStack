const API_VERSION = "v1";

export const API = {
  PRODUCTS: {
    BASE: `/${API_VERSION}/products`,
    ADD_NEW: `/${API_VERSION}/products/AddNewProduct`,
    GET_ALL: `/${API_VERSION}/products/GetAll`,
    GET_DETAILS: (id: string | number) => `/${API_VERSION}/products/${id}`,
    UPDATE: (id: string | number) => `/${API_VERSION}/products/update/${id}`,
    DELETE: (id: string | number) => `/${API_VERSION}/products/delete/${id}`,
  },

  CROPS: {
    BASE: `/${API_VERSION}/crops`,
    ADD_NEW: `/${API_VERSION}/crops/AddNewCrop`,
    GET_ALL: `/${API_VERSION}/crops/GetAll`,
  },

  CATEGORIES: {
    BASE: `/${API_VERSION}/categories`,
    GET_ALL: `/${API_VERSION}/categories/GetAll`,
    CREATE: `/${API_VERSION}/categories/create`,
  },

  USERS: {
    BASE: `/${API_VERSION}/users`,
    LOGIN: `/${API_VERSION}/users/login`,
    REGISTER: `/${API_VERSION}/users/register`,
  },
} as const;
