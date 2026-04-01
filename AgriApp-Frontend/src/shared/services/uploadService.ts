import apiClient from "./apiClient";

export const uploadService = {
  uploadService: async (endpoint: string, files: File[]) => {
    const fd = new FormData();
    files.forEach((f) => fd.append("images", f));
    return apiClient.post(`${endpoint}`, fd).then((r) => r.data);
  },

  uploadSingle: async (endpoint: string, file: File) => {
    const fd = new FormData();
    fd.append("image", file);
    return apiClient.post(`${endpoint}`, fd).then((r) => r.data);
  },
};
