import apiClient from "./apiClient";
export const apiCrudService = {
  getAll: async <T>(endpoint: string): Promise<T[]> =>
    apiClient.get(endpoint).then((r) => r.data),

  getById: async <T>(endpoint: string, id: number): Promise<T> =>
    apiClient.get(`${endpoint}/${id}`).then((r) => r.data),

  create: async <T, R>(endpoint: string, data: T): Promise<R> =>
    apiClient.post(endpoint, data).then((r) => r.data),

  update: async <T, R>(endpoint: string, data: T): Promise<R> =>
    apiClient.put(`${endpoint}`, data).then((r) => r.data),

  delete: async <R = any>(endpoint: string, id: number): Promise<R> =>
    apiClient.delete(`${endpoint}/${id}`).then((r) => r.data),
};
